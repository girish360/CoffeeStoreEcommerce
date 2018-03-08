using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoffeeShop.Entities
{
    public class Orders : BaseID
    {
        public Orders()
        {
            OrderItems = new List<OrderItem>();
        }

        public DateTime OrderDate { get; set; }
        public int Total { get; set; }
        public int Tax { get; set; }
        public int SubTotal { get; set; }
        public int Shipping { get; set; }
        public string PaypalReference { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }

    }
}