using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CoffeeShop.Entities
{
    public class Ticket : BaseID
    {
        [MaxLength(150, ErrorMessage = "Máximo 150 caracteres.")]
        [DataType(DataType.Text)]
        public string Title { get; set; }

        [MaxLength(200, ErrorMessage = "Máximo 200 caracteres.")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Nome obrigatório!")]
        public string Name { get; set; }

        [MaxLength(100, ErrorMessage = "Máximo 100 caracteres.")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage ="E-mail obrigatório!")]
        public string Email { get; set; }

        [MaxLength(8, ErrorMessage = "Máximo 8 caracteres.")]
        public string ZipCode { get; set; }

        [MaxLength(300, ErrorMessage = "Máximo 300 caracteres.")]
        public string Street { get; set; }

        [MaxLength(10, ErrorMessage = "Máximo 10 caracteres.")]
        public string AddressNumber { get; set; }

        [MaxLength(100, ErrorMessage = "Máximo 100 caracteres.")]
        public string City { get; set; }

        [MaxLength(100, ErrorMessage = "Máximo 100 caracteres.")]
        public string Country { get; set; }

        public string PaypalReference { get; set; }

        [ForeignKey("Coffee")]
        public int CoffeeId { get; set; }
        public Coffee Coffee { get; set; }

    }
}