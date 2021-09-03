using eCommerceSite.Data;
using eCommerceSite.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace eCommerceSite.Controllers
{
    public class UserController : Controller
    {

        private readonly ProductContext _context;

        public UserController(ProductContext context)
        {
            _context = context;
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegistrationViewModel reg)
        {
            if (ModelState.IsValid)
            {
                // Check if Email is unique
                bool isEmailTaken = await (from acc in _context.UserAccounts
                                           where acc.Email == reg.Email
                                           select acc).AnyAsync();

                // If email is not Unique error and send abck to view
                if (isEmailTaken)
                {
                    ModelState.AddModelError(nameof(RegistrationViewModel.Email), "That e-mail is already in use.");
                }

                // Check for Username
                bool isUsernameTaken = await (from acc in _context.UserAccounts
                                              where acc.Username == reg.Username
                                              select acc).AnyAsync();

                // If username is not Unique error and send abck to view
                if (isUsernameTaken)
                {
                    ModelState.AddModelError(nameof(RegistrationViewModel.Username), "That username is already in use.");
                }
                if (isEmailTaken || isUsernameTaken)
                {
                    return View(reg);
                }

                UserAccount account = new UserAccount()
                {
                    DateOfBirth = reg.DateOfBirth,
                    Email = reg.Email,
                    Password = reg.Password,
                    Username = reg.Username
                };
                // Add to database
                _context.UserAccounts.Add(account);
                await _context.SaveChangesAsync();

                // Log user in
                LogUserIn(account.UserID);

                // Say Hello!
                TempData["Message"] = $"You have submitted enough for us to take over your life, you've been warned {reg.Username}!";

                // Redirect to home
                return RedirectToAction("Index", "Home");
            }
            return View(reg);



            /*            if (ModelState.IsValid)
                        {
                            UserAccount acc = new UserAccount()
                            {
                                DateOfBirth = reg.DateOfBirth,
                                Email = reg.Email,
                                Password = reg.Password,
                                Username = reg.Username
                            };
                            _context.UserAccounts.Add(acc);
                            await _context.SaveChangesAsync();
                            return RedirectToAction("Index", "Home");
                        }
                        return View(reg);
            */
        }

        public IActionResult Login()
        {
            if (HttpContext.Session.GetInt32("UserId").HasValue)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            UserAccount account = await (from u in _context.UserAccounts
                                         where (u.Username == model.UsernameOrEmail
                                             || u.Email == model.UsernameOrEmail)
                                         && u.Password == model.Password
                                         select u).SingleOrDefaultAsync();


            if (account == null)
            {
                // Credentials did not match
                ModelState.AddModelError(nameof(LoginViewModel.UsernameOrEmail), "That was your last chance to get it right before ''the badness'', please try again.");
                // Error Message
                TempData["Message"] = $"The badness is getting closer.";
                return View(model);
            }
            TempData["Message"] = $"You escaped the badness, good for you...perhaps, {account.Username}!";

            // Log User into website. Do this after adding sessions to start up.
            LogUserIn(account.UserID);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            // Destroy Session
            HttpContext.Session.Clear();

            // Redirect to Homepage
            return RedirectToAction("Index", "Home");
        }

        private void LogUserIn(int accountId)
        {
            HttpContext.Session.SetInt32("UserId", accountId);
        }


    }
}
