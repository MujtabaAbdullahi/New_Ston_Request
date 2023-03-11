using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using New_Ston_Request.Data;
using New_Ston_Request.Models;
using New_Ston_Request.Utility;
using New_Ston_Request.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace New_Ston_Request.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;


        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            HomeVM homeVM = new HomeVM()
            {
                Products = _db.Product.Include(c => c.Category).Include(a => a.ApplicationType),
                Categories = _db.Category

            };
            return View(homeVM);
        }

        public IActionResult Details(int id)
        {
            List<ShoppingCart> shoppingCartsList = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart) != null
               && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Count() > 0)
            {
                shoppingCartsList = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
            }

            DetailsVM detailsVm = new DetailsVM()
            {
                Product = _db.Product.Include(c => c.Category).Include(a => a.ApplicationType).Where(p => p.Id == id).FirstOrDefault(),
                ExistsInCart = false
            };

            foreach(var shopCart in shoppingCartsList)
            {
                if(shopCart.ProductId == id)
                {
                    detailsVm.ExistsInCart = true;
                }
            }

            return View(detailsVm);
        }

        [HttpPost, ActionName("Details")]
        [ValidateAntiForgeryToken]
        public IActionResult DetailsPost(int id)
        {
            List<ShoppingCart> shoppingCartsList = new List<ShoppingCart>();
            if(HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart) != null 
               && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Count() > 0)
            {
                shoppingCartsList = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
            }

            shoppingCartsList.Add(new ShoppingCart { ProductId = id });
            HttpContext.Session.Set(WC.SessionCart, shoppingCartsList);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult RemoveFromCart(int id)
        {
            List<ShoppingCart> shoppingCartsList = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart) != null
               && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Count() > 0)
            {
                shoppingCartsList = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
            }
            var itemToRemove = shoppingCartsList.SingleOrDefault(s => s.ProductId == id);
            if(itemToRemove != null)
            {
                shoppingCartsList.Remove(itemToRemove);
            }

            HttpContext.Session.Set(WC.SessionCart, shoppingCartsList);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
