using Microsoft.AspNetCore.Mvc;
using New_Ston_Request.Data;
using New_Ston_Request.Models;
using System.Collections.Generic;

namespace New_Ston_Request.Controllers
{
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
    }
}
