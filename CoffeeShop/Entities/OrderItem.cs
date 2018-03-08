using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoffeeShop.Entities
{
    public class OrderItem : BaseID
    {
        public int OrderId { get; set; }
        public Orders Order { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
    }
}