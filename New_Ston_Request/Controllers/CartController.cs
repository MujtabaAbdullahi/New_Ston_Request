using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using New_Ston_Request.Data;
using New_Ston_Request.Models;
using New_Ston_Request.Utility;
using New_Ston_Request.ViewModels;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace New_Ston_Request.Controllers
{

    //[Authorize(Roles = WC.AdminRole)]
    [Authorize]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IEmailSender _emailSender;

        [BindProperty]
        public ProductUserVM productUserVM { get; set; }

        public CartController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment,IEmailSender emailSender)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
            _emailSender = emailSender;
        }

        public IActionResult Index()
        {
            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();

            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart) != null
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
                productList = prodList.ToList()
            };


            return View(productUserVM);
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        [ActionName("Summary")]
        public async Task<IActionResult> SummaryPost(ProductUserVM productUserVM)
        {
            var PathToTemplate = _webHostEnvironment.WebRootPath + Path.DirectorySeparatorChar.ToString()
                + "templates" + Path.DirectorySeparatorChar.ToString() + "Inquiry.html";

            var subject = "New Inquiry";
            string HtmlBody = "";
            using (StreamReader sr = System.IO.File.OpenText(PathToTemplate))
            {
                HtmlBody = sr.ReadToEnd();
            }

            //    Name: { 0}

            //    Email: { 1}

            //    Phone Number: { 2}

            //    Products Intereseted:  { 3}

            StringBuilder productListSB = new StringBuilder();
            foreach(var pro in productUserVM.productList)
            {
                productListSB.Append($"- Name : {pro.Name} <span style='font-size: 14px'> (ID : {pro.Id})</span><br/>");
            }

            string messageBody = string.Format(HtmlBody,
                productUserVM.applicationUser.FullName,
                productUserVM.applicationUser.Email,
                productUserVM.applicationUser.PhoneNumber,
                productListSB.ToString());

            await _emailSender.SendEmailAsync(WC.AdminEmail, subject, messageBody);


            return RedirectToAction(nameof(InquryConformation));
        }

        public IActionResult InquryConformation()
        {
            HttpContext.Session.Clear();

            return View();
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
            TempData["success"] = "Product Deleted from Cart Successfully";
            return RedirectToAction(nameof(Index));
        }
    }
}
