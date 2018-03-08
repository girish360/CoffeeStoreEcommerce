using CoffeeShop.Entities;
using CoffeeShop.Models;
using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CoffeeShop.Controllers
{
    public class SimplePaymentController : Controller
    {
        private readonly DbBase _db;
        public SimplePaymentController()
        {
            _db = new DbBase();
        }

        [HttpGet]
        public ActionResult Purchase(int? id)
        {
            //recebe event, trocar 1 pela param id
            var coffee = _db.Coffees.FirstOrDefault(m => m.Id == id);

            var model = new PurchaseVM()
            {
                Coffees = coffee
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Purchase(PurchaseVM model)
        {
            if (ModelState.IsValid)
            {
                //recebe o event
                var ev = _db.Coffees.FirstOrDefault(m => m.Id == model.Coffees.Id);

                //cria ticket
                var ticket = new Ticket()
                {
                    Name = model.Tickets.Name,
                    Email = model.Tickets.Email,
                    ZipCode = model.Tickets.ZipCode,
                    Street = model.Tickets.Street,
                    AddressNumber = model.Tickets.AddressNumber,
                    City = model.Tickets.City,
                    Country = model.Tickets.Country,
                    CoffeeId = model.Coffees.Id
                };

                _db.Tickets.Add(ticket);
                _db.SaveChanges();

                //cria objeto pagamento
                var payment = new Payment
                {
                    //id gerado pelo controller WebExperience
                    experience_profile_id = "XP-ZPYR-5PG7-UPN3-QBJQ",
                    intent = "sale",
                    payer = new Payer
                    {
                        payment_method = "paypal"
                    },
                    transactions = new List<Transaction>
                    {
                        new Transaction
                        {
                            description = "User: cookeryappninja-buyer@gmail.com Senha:mar982836",
                            amount = new Amount
                            {
                                currency = "BRL",
                                total = ev.Price.ToString()
                            },
                            item_list = new ItemList()
                            {
                                items = new List<Item>()
                                {
                                    new Item
                                    {
                                        description = $"Evento: {model.Coffees.Name} Preço: {model.Coffees.Price.ToString()}",
                                        currency = "BRL",
                                        quantity = "1",
                                        price = ev.Price.ToString()
                                    }
                                }
                            }
                        }
                    },
                    redirect_urls = new RedirectUrls
                    {
                        return_url = Url.Action("Return", "SimplePayment", null, Request.Url.Scheme),
                        cancel_url = Url.Action("Cancel", "SimplePayment", null, Request.Url.Scheme)
                    }
                };

                //cria api context do paypal com as credências(paypal developer site) em web.config
                var apiContext = GetAPIContext();

                //envia o objeto pagamento para o paypal api
                var createdPayment = payment.Create(apiContext);

                //salva a referência do pagamento no ticket, referência criada pelo paypal api, e salva no DB
                ticket.PaypalReference = createdPayment.id;
                _db.SaveChanges();

                //procura a url no createdPayment(objeto pagamento paypal), o mesmo retorna Return ou Cancel, se ok envia para page de login do paypal :)
                var approvalUrl = createdPayment.links.FirstOrDefault(m => m.rel.Equals("approval_url", StringComparison.OrdinalIgnoreCase));

                //envia tela para validar dados do user, e finalizar compra com o paypal
                return Redirect(approvalUrl.href);
            }
            else
            {
                var c = _db.Coffees.Find(model.Coffees.Id);
                var m = new PurchaseVM
                {
                    Coffees = c
                };
                return View(m);
            }
        }

        public ActionResult Return(string payerId, string paymentId)
        {
            //busca o ticket criado pela referência do pagamento no ticket PaypalReference
            var ticket = _db.Tickets.FirstOrDefault(m => m.PaypalReference == paymentId);
            var ev = _db.Coffees.Find(ticket.CoffeeId);
            //popula purchaseVM
            var PVM = new PurchaseVM
            {
                Coffees = ev,
                Tickets = ticket
            };

            //obtém Paypal api context
            var apiContext = GetAPIContext();

            //configura o pagador com o pagamento
            var paymentExecution = new PaymentExecution()
            {
                payer_id = payerId
            };

            //cria payment para exe
            var payment = new Payment()
            {
                id = paymentId
            };

            //exe pagamento
            var executedPayment = payment.Execute(apiContext, paymentExecution);

            ////envia email, DONE, envia email test!
            //const string emailFrom = "cookeryappninja@gmail.com";
            //Mail(ticket.Email, emailFrom, PVM);

            //direciona para view thank's , end of the line 
            return RedirectToAction("ThankY");
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