using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication16.Constants;
using WebApplication16.ViewModels;

namespace WebApplication16.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize]
    public class UsersController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: Admin/Users
        [Authorize(Policy = Permissions.Users.View)]
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            return View(users);
        }

        // GET: Admin/Users/ManageRoles?userId=...
        [HttpGet]
        [Authorize(Policy = Permissions.Users.ManageRoles)]
        public async Task<IActionResult> ManageRoles(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var allRoles = await _roleManager.Roles.ToListAsync();
            var userRoles = await _userManager.GetRolesAsync(user);

            var model = new UserRolesViewModel
            {
                UserId = userId,
                UserName = user.UserName,
                Roles = allRoles.Select(role => new RoleViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name,
                    IsSelected = userRoles.Contains(role.Name)
                }).ToList()
            };

            return View(model);
        }

        // POST: Admin/Users/ManageRoles
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = Permissions.Users.ManageRoles)]
        public async Task<IActionResult> ManageRoles(UserRolesViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return NotFound();
            }

            var currentRoles = await _userManager.GetRolesAsync(user);

            // حذف تمام نقش‌های قبلی کاربر
            await _userManager.RemoveFromRolesAsync(user, currentRoles);

            // افزودن نقش‌های جدید انتخاب شده
            var selectedRoles = model.Roles.Where(r => r.IsSelected).Select(r => r.RoleName);
            await _userManager.AddToRolesAsync(user, selectedRoles);

            return RedirectToAction(nameof(Index));
        }
    }
}