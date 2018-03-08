using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoffeeShop.Models.ECommerce
{
    public class CartItem
    {
        public int CoffeeId { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }

    }
}