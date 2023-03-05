using Microsoft.AspNetCore.Mvc;
using New_Ston_Request.Data;
using New_Ston_Request.Models;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace New_Ston_Request.Controllers
{
    public class CategoryController : Controller
    {

        private readonly ApplicationDbContext _db;

        public CategoryController(ApplicationDbContext db )
        {
            _db = db;
        }
        public IActionResult Index()
        {
            IEnumerable<Category> categoryList = _db.Category;
            return View(categoryList);
        }

        //GET Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        //POST Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category obj)
        {
            if (ModelState.IsValid)
            {
                _db.Category.Add(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
                return View(obj);
        }
    }
}
