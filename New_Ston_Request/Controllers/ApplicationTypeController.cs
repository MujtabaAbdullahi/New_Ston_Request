using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using New_Ston_Request.Data;
using New_Ston_Request.Models;
using System.Collections.Generic;
using System.Data;

namespace New_Ston_Request.Controllers
{
    [Authorize(Roles = WC.AdminRole)]
    public class ApplicationTypeController : Controller
    {
        private readonly ApplicationDbContext _db;
        public ApplicationTypeController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            IEnumerable<ApplicationType> appList = _db.ApplicationType;
            return View(appList);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ApplicationType applist)
        {
            if (ModelState.IsValid)
            {
                _db.ApplicationType.Add(applist);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
                return View(applist);
        }

        // Get specific Application type by it's id
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var appType = _db.ApplicationType.Find(id);
            if (appType == null)
            {
                return NotFound();
            }
            return View(appType);
        }

        // Edit specific Application type by it's id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ApplicationType appType)
        {
            if (ModelState.IsValid)
            {
                _db.ApplicationType.Update(appType);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
                return View(appType);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var appType = _db.ApplicationType.Find(id);
            if (appType == null)
            {
                return NotFound();
            }
            return View(appType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var appType = _db.ApplicationType.Find(id);
            if (appType == null)
            {
                return NotFound();
            }
            _db.ApplicationType.Remove(appType);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }


    }
}
