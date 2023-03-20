using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using New_Ston_Request.Data;
using New_Ston_Request.Models;
using New_Ston_Request.Utility;
using New_Ston_Request.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace New_Ston_Request.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ProductUserVM productUserVM { get; set; }

        public CartController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();

            if(HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart) != null 
                && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Count() > 0)
            {
                shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
            }

            List<int> cartList = shoppingCartList.Select(c => c.ProductId).ToList();
            IEnumerable<Product> prodList = _db.Product.Where(p => cartList.Contains(p.Id)).ToList();

            return View(prodList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Index")]
        public IActionResult IndexPost()
        {
            return RedirectToAction(nameof(Summary));
        }
        public IActionResult Summary()
        {
            // for finding loged user
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            // or
            //var userId = User.FindFirst(ClaimTypes.Name);
            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();

            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Count() > 0)
            {
                shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
            }

            List<int> cartList = shoppingCartList.Select(c => c.ProductId).ToList();
            IEnumerable<Product> prodList = _db.Product.Where(p => cartList.Contains(p.Id)).ToList();

            productUserVM = new ProductUserVM()
            {
                applicationUser = _db.ApplicationUser.FirstOrDefault(a => a.Id == claim.Value),
                productList =prodList
            };


            return View(productUserVM);
        }

        public IActionResult RemoveCart(int id)
        {
            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();

            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Count() > 0)
            {
                shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
            }
            shoppingCartList.Remove(shoppingCartList.FirstOrDefault(p => p.ProductId == id));
            HttpContext.Session.Set(WC.SessionCart, shoppingCartList);

            return RedirectToAction(nameof(Index));
        }
    }
}
