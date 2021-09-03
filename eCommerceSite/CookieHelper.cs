using eCommerceSite.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerceSite
{
    public class CookieHelper //AKA the grandkids
    {
        const string CartCookie = "CartCookie";

        /// <summary>
        /// Returns a list of cart products. If cart is empty and empty List<> will be returned.
        /// </summary>
        /// <param name="http"></param>
        /// <returns>A list of products or Empty list if cart is empty.</returns>
        public static List<Product> GetCartProducts(IHttpContextAccessor http)
        {
            // Get exsisting cart items
            string exsistingCart = http.HttpContext.Request.Cookies[CartCookie];
            List<Product> cartProducts = new List<Product>();
            if (exsistingCart != null)
            {
                cartProducts = JsonConvert.DeserializeObject<List<Product>>(exsistingCart); //who's Json? I don't know him and you leave him alone, he can believe what he wants!
            }
            return cartProducts;
        }

        public static void AddProductToCart(IHttpContextAccessor http, Product p)
        {
            List<Product> cartProducts = GetCartProducts(http);
            cartProducts.Add(p);

            string data = JsonConvert.SerializeObject(cartProducts);

            // Cookie Options
            CookieOptions options = new CookieOptions() // Chocolate? Only if it's in chip form.  Chocolate cookies are the Devil!
            {
                Expires = DateTime.Now.AddYears(1),
                Secure = true,
                IsEssential = true
            };

            http.HttpContext.Response.Cookies.Append(CartCookie, data, options);
        }

        public static int GetNumOFCartProducts(IHttpContextAccessor http)
        {
            List<Product> cartProducts = GetCartProducts(http);
            return cartProducts.Count;
        }
    }
}
