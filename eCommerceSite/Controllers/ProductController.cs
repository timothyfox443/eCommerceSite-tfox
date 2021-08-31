using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using eCommerceSite.Data;
using eCommerceSite.Models;
using Microsoft.AspNetCore.Mvc;

namespace eCommerceSite.Controllers
{
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    public class ProductController : Controller
    {
        private readonly ProductContext _context;

        public ProductController(ProductContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Displays a view that lists all products
        /// </summary>
        public IActionResult Index(int? id)
        {
            // Get all products from database
            // List<Product> products = _context.Products.ToList();
            List<Product> products =
                (from p in _context.Products
                 select p).ToList();
            

            // Send list of products to view to be displayed
            return View(products);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(Product p)
        {
            if(ModelState.IsValid)
            {
                _context.Products.Add(p);
                _context.SaveChanges();
                TempData["Message"] = $"{p.Title} was born!";

                return RedirectToAction("index");
            }
            return View();
        }

        private string GetDebuggerDisplay()
        {
            return ToString();
        }//delete goes somewhere
    }
}
