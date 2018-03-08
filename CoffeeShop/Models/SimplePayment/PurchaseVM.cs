using CoffeeShop.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoffeeShop.Models
{
    public class PurchaseVM
    {
        public Coffee Coffees { get; set; }
        public Ticket Tickets { get; set; }
    }
}