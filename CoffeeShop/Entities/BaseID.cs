using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CoffeeShop.Entities
{
    public class BaseID
    {
        [Key]
        public int Id { get; set; }
    }
}