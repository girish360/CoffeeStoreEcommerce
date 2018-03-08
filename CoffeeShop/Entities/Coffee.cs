using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CoffeeShop.Entities
{
    public class Coffee : BaseID
    {
        [StringLength(150,ErrorMessage ="No máximo 150 caracteres.")]
        public string Name { get; set; }
        [StringLength(300, ErrorMessage = "No máximo 300 caracteres.")]
        [DataType(DataType.ImageUrl)]
        public string ImageUrl { get; set; }
        [DataType(DataType.Currency)]
        public int Price { get; set; }
        public bool IsTheBest { get; set; }
        [StringLength(1700, ErrorMessage = "No máximo 1700 caracteres.")]
        public string LongDescription { get; set; }
        [StringLength(250, ErrorMessage = "No máximo 250 caracteres.")]
        public string ShortDescription { get; set; }
    }
}