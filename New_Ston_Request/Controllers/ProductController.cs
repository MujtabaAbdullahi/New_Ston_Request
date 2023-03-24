using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using New_Ston_Request.Data;
using New_Ston_Request.Models;
using New_Ston_Request.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace New_Ston_Request.Controllers
{
    [Authorize(Roles = WC.AdminRole)]
    public class ProductController : Controller
    {

        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            IEnumerable<Product> productlist = _db.Product.Include(c => c.Category).Include(a => a.ApplicationType);
            //foreach(var product in productlist)
            //{
            //    product.Category = _db.Category.FirstOrDefault(u => u.Id == product.CategoryId);
            //    //product.Category = _db.Category.Select(u => u).Where(u => u.Id == product.CategoryId).SingleOrDefault();
            //};
            return View(productlist);
        }

        //GET Upsert
        [HttpGet]
        public IActionResult Upsert(int? id)
        {
            //IEnumerable<SelectListItem> CategoryDropdown = _db.Category.Select(i => new SelectListItem
            //{
            //    Text = i.Name,
            //    Value = i.Id.ToString()
            //});
            ////ViewBag.CategoryDropdown = CategoryDropdown;
            //ViewData["CategoryDropdown"] = CategoryDropdown;
            //Product product = new Product();
            ProductVM productVM = new ProductVM()
            {
                Product = new Product(),
                CategorySelectList = _db.Category.Select(i => new SelectListItem()
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                ApplicationTypeList = _db.ApplicationType.Select(i => new SelectListItem()
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };

            if (id == null)
            {
                return View(productVM);
            }
            else
            {
                productVM.Product = _db.Product.Find(id);
                if (productVM.Product == null)
                {
                    return NotFound();
                }
                return View(productVM);
            }
        }

        //POST Upsert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM productVM)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                string webRootPath = _webHostEnvironment.WebRootPath;


                if (productVM.Product.Id == 0)
                {
                    // Creating
                    string upload = webRootPath + WC.ProductImagePath;
                    string fileName = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(files[0].FileName);

                    using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    };

                    productVM.Product.Image = fileName + extension;
                    _db.Product.Add(productVM.Product);
                    TempData["success"] = "Product Created Successfully";
                }
                else
                {
                    // Updating
                    var oldProduct = _db.Product.AsNoTracking().FirstOrDefault(p => p.Id == productVM.Product.Id);
                    //var oldProduct = _db.Product.AsNoTracking().Select(p => p).Where(p => p.Id == productVM.Product.Id).SingleOrDefault();

                    if (files.Count > 0)
                    {
                        string upload = webRootPath + WC.ProductImagePath;
                        string fileName = Guid.NewGuid().ToString();
                        string extension = Path.GetExtension(files[0].FileName);

                        //for deleting old image
                        var oldImage = Path.Combine(upload, oldProduct.Image);
                        if (System.IO.File.Exists(oldImage))
                        {
                            System.IO.File.Delete(oldImage);
                        }

                        using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                        };
                        productVM.Product.Image = fileName + extension;
                    }
                    else
                    {
                        productVM.Product.Image = oldProduct.Image;
                    }
                    _db.Product.Update(productVM.Product);
                    TempData["success"] = "Product Updated Successfully";
                }

                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
                productVM.CategorySelectList = _db.Category.Select(i => new SelectListItem()
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
            productVM.ApplicationTypeList = _db.ApplicationType.Select(i => new SelectListItem()
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
            return View(productVM);
        }


        //For deleting product
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Product product = _db.Product.Include(c => c.Category).Include(a => a.ApplicationType).Where(u => u.Id == id).FirstOrDefault();
            //product.Category = _db.Category.Find(product.CategoryId);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var product = _db.Product.Find(id);
            if (product == null)
            {
                TempData["error"] = "Sorry Someting went Wrong!";
                return NotFound();
            }
            string upload = _webHostEnvironment.WebRootPath + WC.ProductImagePath;

            //for deleting old image
            var productImage = Path.Combine(upload, product.Image);
            if (System.IO.File.Exists(productImage))
            {
                System.IO.File.Delete(productImage);
            }

            TempData["success"] = "Product Deleted Successfully";
            _db.Product.Remove(product);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
