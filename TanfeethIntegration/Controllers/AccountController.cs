using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TanfeethIntegration.Data;
using TanfeethIntegration.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using TanfeethIntegration.Services;
using System.Security.Claims;

namespace TanfeethIntegration.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly LogDbContext _dbContext;
        private readonly UserService _userService;

        public AccountController(ILogger<AccountController> logger, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, LogDbContext dbContext, UserService userService)
        {
            _logger = logger;
            _signInManager = signInManager;
            _userManager = userManager;
            _dbContext = dbContext;
            _userService = userService;
        }

        private async Task<bool> IsUserAdmin()
        {
            var currentUser = await _userService.GetCurrentUserAsync(User);
            return currentUser?.IsAdmin ?? false;
        }

        // Create
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!await IsUserAdmin())
            {
                return Forbid();
            }

            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Username,
                    FullName = model.FullName,
                    IsActive = model.IsActive,
                    IsAdmin = model.IsAdmin,
                    CreationDate = DateTime.Now,
                    LastUpdate = DateTime.Now
                };
                var result = await _userManager.CreateAsync(user, "DefaultPassword@123"); // Set a default password or generate a random one

                if (result.Succeeded)
                {
                    if (user.IsAdmin)
                    {
                        await _userManager.AddClaimAsync(user, new Claim("IsAdmin", "true"));
                    }
                    _logger.LogInformation("Admin created a new account.");
                    return RedirectToAction("Index");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        // Read
        [HttpGet]
        public async Task<IActionResult> Register()
        {
            if (!await IsUserAdmin())
            {
                return Forbid();
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ToggleActive(string id)
        {
            if (!await IsUserAdmin())
            {
                return Forbid();
            }

            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            user.IsActive = !user.IsActive;
            user.LastUpdate = DateTime.Now;
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return RedirectToAction(nameof(Index));
        }

        // Read
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (!await IsUserAdmin())
            {
                return Forbid();
            }

            var users = _userManager.Users.ToList();
            return View(users);
        }

        // Update
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (!await IsUserAdmin())
            {
                return Forbid();
            }

            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var model = new RegisterViewModel
            {
                FullName = user.FullName,
                Username = user.UserName,
                IsActive = user.IsActive,
                IsAdmin = user.IsAdmin
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, RegisterViewModel model)
        {
            if (!await IsUserAdmin())
            {
                return Forbid();
            }

            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                user.FullName = model.FullName;
                user.UserName = model.Username;
                user.IsActive = model.IsActive;
                user.IsAdmin = model.IsAdmin;

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    if (user.IsAdmin)
                    {
                        await _userManager.AddClaimAsync(user, new Claim("IsAdmin", "true"));
                    }
                    else
                    {
                        var claims = await _userManager.GetClaimsAsync(user);
                        var adminClaim = claims.FirstOrDefault(c => c.Type == "IsAdmin");
                        if (adminClaim != null)
                        {
                            await _userManager.RemoveClaimAsync(user, adminClaim);
                        }
                    }
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        // Delete
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            if (!await IsUserAdmin())
            {
                return Forbid();
            }

            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
