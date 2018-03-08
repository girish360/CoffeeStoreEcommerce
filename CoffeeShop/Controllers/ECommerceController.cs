using CoffeeShop.Entities;
using CoffeeShop.Models;
using CoffeeShop.Models.ECommerce;
using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CoffeeShop.Controllers
{
    public class ECommerceController : Controller
    {
        private readonly DbBase _db;
        public ECommerceController()
        {
            _db = new DbBase();
        }

        // GET: ECommerce
        public ActionResult Cart()
        {
            var cart = CreateOfGetCart();

            return View(cart);
        }

        public ActionResult Add(int id)
        {
            var coffee = _db.Coffees.FirstOrDefault(x => x.Id == id);

            var cart = CreateOfGetCart();
            var existingItem = cart.CartItems.FirstOrDefault(x => x.CoffeeId == coffee.Id);

            if (existingItem != null)
            {
                existingItem.Quantity++;
            }
            else
            {
                cart.CartItems.Add(new CartItem()
                {
                    CoffeeId = coffee.Id,
                    Name = coffee.Name,
                    Price = coffee.Price,
                    Quantity = 1
                });
            }

            SaveCart(cart);

            return RedirectToAction("Cart", "ECommerce");
        }

        public ActionResult Delete(int beerId)
        {
            var beer = _db.Coffees.FirstOrDefault(x => x.Id == beerId);

            var cart = CreateOfGetCart();
            var existingItem = cart.CartItems.FirstOrDefault(x => x.CoffeeId == beer.Id);

            if (existingItem != null)
            {
                cart.CartItems.Remove(existingItem);
            }

            SaveCart(cart);

            return RedirectToAction("Cart", "ECommerce");
        }

        public ActionResult Checkout()
        {
            var cart = CreateOfGetCart();

            if (cart.CartItems.Any())
            {
                // Flat rate shipping
                int shipping = 500;

                // Flat rate tax 10%
                var taxRate = 0.1M;

                var subtotal = cart.CartItems.Sum(x => x.Price * x.Quantity);
                var tax = Convert.ToInt32((subtotal + shipping) * taxRate);
                var total = subtotal + shipping + tax;

                // Create an Order object to store info about the shopping cart
                var order = new CoffeeShop.Entities.Orders()
                {
                    OrderDate = DateTime.UtcNow,
                    SubTotal = subtotal,
                    Shipping = shipping,
                    Tax = tax,
                    Total = total,
                    OrderItems = cart.CartItems.Select(x => new OrderItem()
                    {
                        Name = x.Name,
                        Price = x.Price,
                        Quantity = x.Quantity
                    }).ToList()
                };
                _db.Orders.Add(order);
                _db.SaveChanges();

                // Get PayPal API Context using configuration from web.config
                var apiContext = GetAPIContext();

                // Create a new payment object
                var payment = new Payment
                {
                    intent = "sale",
                    payer = new Payer
                    {
                        payment_method = "paypal"
                    },
                    transactions = new List<Transaction>
                    {
                        new Transaction
                        {
                            description = $"BeerPal Brewery Shopping Cart Purchase",
                            amount = new Amount
                            {
                                currency = "BRL",
                                total = order.Total.ToString(), // PayPal expects string amounts, eg. "20.00",
                                details = new Details()
                                {
                                    subtotal = order.SubTotal.ToString(),
                                    shipping = order.Shipping.ToString(),
                                    tax = order.Tax.ToString()
                                }
                            },
                            item_list = new ItemList()
                            {
                                items =
                                    order.OrderItems.Select(x => new Item()
                                    {
                                        description = x.Name,
                                        currency = "BRL",
                                        quantity = x.Quantity.ToString(),
                                        price = x.Price.ToString(), // PayPal expects string amounts, eg. "20.00"
                                    }).ToList()
                            }
                        }
                    },
                    redirect_urls = new RedirectUrls
                    {
                        return_url = Url.Action("Return", "ECommerce", null, Request.Url.Scheme),
                        cancel_url = Url.Action("Cancel", "ECommerce", null, Request.Url.Scheme)
                    }
                };

                // Send the payment to PayPal
                var createdPayment = payment.Create(apiContext);

                // Save a reference to the paypal payment
                order.PaypalReference = createdPayment.id;
                _db.SaveChanges();

                // Find the Approval URL to send our user to
                var approvalUrl =
                    createdPayment.links.FirstOrDefault(
                        x => x.rel.Equals("approval_url", StringComparison.OrdinalIgnoreCase));

                // Send the user to PayPal to approve the payment
                return Redirect(approvalUrl.href);
            }

            return RedirectToAction("Cart");
        }

        public ActionResult Return(string payerId, string paymentId)
        {
            // Fetch the existing order
            var order = _db.Orders.FirstOrDefault(x => x.PaypalReference == paymentId);

            // Get PayPal API Context using configuration from web.config
            var apiContext = GetAPIContext();

            // Set the payer for the payment
            var paymentExecution = new PaymentExecution()
            {
                payer_id = payerId
            };

            // Identify the payment to execute
            var payment = new Payment()
            {
                id = paymentId
            };

            // Execute the Payment
            var executedPayment = payment.Execute(apiContext, paymentExecution);

            ClearCart();

            return RedirectToAction("ThankY");
        }

        public ActionResult Cancel()
        {
            return View();
        }

        public ActionResult ThankY()
        {
            return View();
        }

        private Cart CreateOfGetCart()
        {
            var cart = Session["Cart"] as Cart;
            if (cart == null)
            {
                cart = new Cart();
                SaveCart(cart);
            }

            return cart;
        }

        private void SaveCart(Cart cart)
        {
            Session["Cart"] = cart;
        }

        private void ClearCart()
        {
            var cart = new Cart();
            SaveCart(cart);
        }

        private APIContext GetAPIContext()
        {
            //authenticate for paypal ninja :P
            var config = ConfigManager.Instance.GetProperties();
            var accessToekn = new OAuthTokenCredential(config).GetAccessToken();
            var apicontext = new APIContext(accessToekn);

            return apicontext;
        }
    }
}