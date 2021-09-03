using eCommerceSite.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerceSite.Data
{

    public static class ProductDb
    {
        /// <summary>
        /// returns a total count of products
        /// </summary>
        /// <param name="_context"> Database context to use</param>
        /// <returns></returns>
        public async static Task<int> GetTotalProdsAsync(ProductContext _context)
        {
            return await (from p in _context.Products
                          select p).CountAsync();
        }

        /// <summary>
        /// get a page worth of products
        /// </summary>
        /// <param name="_context">DB context</param>
        /// <param name="pageSize"> number of products per page</param>
        /// <param name="pageNum"> the number of the page</param>
        /// <returns></returns>
        public async static Task<List<Product>> GetProductsAsync(ProductContext _context, int pageSize, int pageNum)
        {
            return await (from p in _context.Products
                          orderby p.Title ascending
                          select p)
                       .Skip(pageSize * (pageNum - 1)) //pageNum - 1 because arraymath
                       .Take(pageSize) // skip before take
                       .ToListAsync();
        }

        /// <summary>
        /// adds a product to the DB
        /// </summary>
        /// <param name="_context">DB context</param>
        /// <param name="p">product to add</param>
        /// <returns></returns>
        public async static Task<Product> AddProductAsync(ProductContext _context, Product p)
        {
            _context.Products.Add(p);
            await _context.SaveChangesAsync();
            return p;
        }

        public static async Task<Product> GetSingleProductAsync(ProductContext context, int prodId)
        {
            Product p = await (from products in context.Products
                               where products.ProductId == prodId
                               select products).SingleOrDefaultAsync();
            return p;
        }
    }
}

