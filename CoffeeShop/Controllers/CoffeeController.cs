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
    public class CoffeeController : Controller
    {
        //injeçao de dependência no ctor
        private readonly DbBase _db;
        public CoffeeController()
        {
            _db = new DbBase();
        }

        public ActionResult BestSellers()
        {
            var vm = new BestSellersVM
            {
                Coffes = GetAllBest()
            };

            return View(vm);
        }

        //[HttpPost]
        //public ActionResult BestSellers(BestSellersVM model)
        //{
        //    return View();
        //}

        [HttpGet]
        public ActionResult AllCoffees()
        {
            var model = new AllCoffeesVM
            {
                Coffee = _db.Coffees.ToList()
            };

            return View(model);
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var model = new DetailsVM
            {
                Coffee = _db.Coffees.Find(id)
            };

            return View(model);
        }

        //method para retornar todos os cafes em lista para view AllCoffees
        public List<Coffee> GetAllCoffees()
        {
            return _db.Coffees.ToList(); 
        }

        //method para retornar somente os the best coffee, para view BestSellers
        public List<Coffee> GetAllBest()
        {
            var query = from c in _db.Coffees
                        where c.IsTheBest == true
                        select c;

            return query.ToList();
        }
    }
}