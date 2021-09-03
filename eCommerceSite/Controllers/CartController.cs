using eCommerceSite.Data;
using eCommerceSite.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerceSite.Controllers
{
    public class CartController : Controller
    {
        private readonly ProductContext _context;
        private readonly IHttpContextAccessor _httpContext;
        public CartController(ProductContext context, IHttpContextAccessor httpContext)
        {
            _context = context;
            _httpContext = httpContext;
        }

        /// <summary>
        /// Add a product to the shopping cart
        /// </summary>
        /// <param name="id">The Id of the product to add</param>
        /// <returns></returns>
        public async Task<IActionResult> Add(int id, string prevUrl)
        {
            Product p = await ProductDb.GetSingleProductAsync(_context, id);

            // Add current product to exsisting cart
            CookieHelper.AddProductToCart(_httpContext, p);
            TempData["Message"] = $"{p.Title} added successfully to your cart.";

            // redirect to preivious page
            return Redirect(prevUrl);
        }

        public IActionResult Summary()
        {
            // Display all products in the shopping cart cookie
            List<Product> cartProducts = CookieHelper.GetCartProducts(_httpContext);
            return View(cartProducts);
        }
    }
}