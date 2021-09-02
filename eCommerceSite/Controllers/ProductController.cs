using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using eCommerceSite.Data;
using eCommerceSite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        /// displays PageSize products per page
        /// </summary>
        public async Task<IActionResult> Index(int? id) //check this argument (int? id)
        {

            int pageNum = id ?? 1;
            const int PageSize = 3;
            ViewData["CurrentPage"] = pageNum;
            int numProds = await (from p in _context.Products
                                  select p).CountAsync();
            int totalPages = (int)Math.Ceiling((double)numProds / PageSize);

            ViewData["MaxPage"] = totalPages;
            // Get all products from database
            // List<Product> products = _context.Products.ToList();
            List<Product> products =
                await (from p in _context.Products
                       orderby p.Title ascending
                       select p)
                       .Skip(PageSize * (pageNum - 1)) //pageNum - 1 because arraymath
                       .Take(PageSize) // skip before take
                       .ToListAsync();


            // Send list of products to view to be displayed
            return View(products); // this returns the product index.cshtml page
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        /// <summary>
        /// This adds an item to the database and upon success displays a success message
        /// </summary>
        /// <param name="p"></param>
        /// <returns> View() </returns>
        [HttpPost]
        public async Task<IActionResult> Add(Product p)
        {
            if (ModelState.IsValid)
            {
                //adds product to DB
                _context.Products.Add(p);

                //
                await _context.SaveChangesAsync();

                TempData["Message"] = $"{p.Title} was born!";

                //redirects back to catalog page view
                return RedirectToAction("index");
            }
            return View(); // returns add.cshtml in product view
        }

        private string GetDebuggerDisplay()
        {
            return ToString();
        }//delete goes somewhere

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            // get product with corresponding id
            Product p =
                await (from prod in _context.Products
                       where prod.ProductId == id
                       select prod).SingleAsync();
            return View(p);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Product p)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(p).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                ViewData["Message"] = "This product entry was altered";
            }
            return View(p);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            Product p = await (from prod in _context.Products
                               where prod.ProductId == id
                               select prod).SingleAsync();

            return View(p);
        }

        [HttpPost]
        [ActionName ("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Product p = await(from prod in _context.Products
                              where prod.ProductId == id
                              select prod).SingleAsync();

            _context.Entry(p).State = EntityState.Deleted;
            await _context.SaveChangesAsync();

            TempData["Message"] = $"{p.Title} sleeps with the fishes!";

            return RedirectToAction("Index");
        }

    }
}
