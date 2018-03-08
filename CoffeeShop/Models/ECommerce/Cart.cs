using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoffeeShop.Models.ECommerce
{
    public class Cart
    {
        public Cart()
        {
            CartItems = new List<CartItem>();
        }

        public List<CartItem> CartItems { get; set; }
    }
}