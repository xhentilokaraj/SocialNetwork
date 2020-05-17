using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Data;
using SocialNetwork.Models;
using Microsoft.Extensions.Logging;
using System.Net.Mail;
using System.Net.Mime;

namespace SocialNetwork.Controllers
{
    public class LoginController : Controller
    {
        private readonly SocialNetworkContext _context;
        private readonly ILogger _logger;

        public LoginController(SocialNetworkContext context, ILogger<LoginController> logger)
        {
            _context = context;
        }

        // GET: /Login/Create
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(string Email, string Password)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.User
                .FirstOrDefaultAsync(m => m.Email == Email && m.Password == Password);
                System.Diagnostics.Debug.WriteLine(user);
                if (user == null)
                {
                    return RedirectToAction("Index", "Login");
                }
                else
                {
                    var userClaims = new List<Claim>()
                    {
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(ClaimTypes.NameIdentifier, user.Email),
                    new Claim("UserProfileImage", user.ProfilePicture),
                    new Claim(ClaimTypes.PrimarySid, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.UserType)
                    };

                    var uIdentity = new ClaimsIdentity(userClaims, "User Identity");

                    var authProperties = new AuthenticationProperties
                    {
                        AllowRefresh = true,
                        ExpiresUtc = DateTimeOffset.Now.AddDays(1),
                        IsPersistent = true,
                    };

                    var userPrincipal = new ClaimsPrincipal(new[] { uIdentity });
                    await HttpContext.SignInAsync("CookieAuthentication", userPrincipal, authProperties);
                }

                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            //_logger.LogInformation("User {Name} logged out at {Time}.", User.Identity.Name, DateTime.UtcNow);

            await HttpContext.SignOutAsync("CookieAuthentication");

            return RedirectToAction("Index", "Home");
        }



    }
}
