using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.DirectoryServices.AccountManagement;
using TanfeethIntegration.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;

namespace TanfeethIntegration.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _logger = logger;
            _signInManager = signInManager;
            _userManager = userManager;
            _configuration = configuration;
        }
        [HttpPost]
        public IActionResult KeepAlive()
        {
            return Ok();
        }

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "LegalProceeding");
            }
            return RedirectToAction("Login");
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // LDAP Authentication
            var domain = _configuration["ActiveDirectory:Domain"];
            var serviceAccountUsername = _configuration["ActiveDirectory:ServiceAccountUsername"];
            var serviceAccountPassword = _configuration["ActiveDirectory:ServiceAccountPassword"];

            if (string.IsNullOrWhiteSpace(domain))
            {
                _logger.LogError("Active Directory configuration is missing.");
                ModelState.AddModelError(string.Empty, "إعدادات الاتصال بخدمة الدليل النشط غير مكتملة.");
                return View(model);
            }

            using (var context = string.IsNullOrWhiteSpace(serviceAccountUsername)
                ? new PrincipalContext(ContextType.Domain, domain)
                : new PrincipalContext(ContextType.Domain, domain, serviceAccountUsername, serviceAccountPassword))
            {
                bool isValid = context.ValidateCredentials(model.Username, model.Password);
                if (isValid)
                {
                    var normalizedUsername = model.Username.ToUpperInvariant();
                    var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == normalizedUsername);

                    if (user == null)
                    {
                        ModelState.AddModelError(string.Empty, "المستخدم غير مسجل. الرجاء الاتصال بمسؤول النظام.");
                        return View(model);
                    }

                    if (!user.IsActive)
                    {
                        ModelState.AddModelError(string.Empty, "المستخدم غير نشط. الرجاء الاتصال بمسؤول النظام.");
                        return View(model);
                    }

                    await _signInManager.SignInAsync(user, model.RememberMe);
                    return RedirectToAction("Index", "LegalProceeding");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "محاولة تسجيل دخول غير صالحة.");
                    return View(model);
                }
            }
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
