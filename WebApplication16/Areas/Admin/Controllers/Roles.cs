using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication16.Constants;
using WebApplication16.ViewModels;

namespace WebApplication16.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize] // در آینده می‌توانیم این را به یک Policy خاص برای مدیریت نقش‌ها محدود کنیم
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;


        //public RolesController(RoleManager<IdentityRole> roleManager) => _roleManager = roleManager;

        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager) // اضافه کردن پارامتر
        {
            _roleManager = roleManager;
            _userManager = userManager; // مقداردهی
        }


        // GET: Admin/Roles
        public async Task<IActionResult> Index()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return View(roles);
        }
        // GET: Admin/Roles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Roles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IdentityRole role)
        {
            if (ModelState.IsValid)
            {
                // بررسی می‌کنیم که آیا نقشی با این نام از قبل وجود دارد یا خیر
                if (!await _roleManager.RoleExistsAsync(role.Name))
                {
                    await _roleManager.CreateAsync(new IdentityRole(role.Name));
                    TempData["SuccessMessage"] = "نقش با موفقیت ایجاد شد.";
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("Name", "نقشی با این نام از قبل وجود دارد.");
            }
            return View(role);
        }
        // GET: Admin/Roles/ManagePermissions?roleId=...
        [HttpGet]
        public async Task<IActionResult> ManagePermissions(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                return NotFound();
            }

            var roleClaims = await _roleManager.GetClaimsAsync(role);
            var allPermissions = typeof(Permissions).GetNestedTypes()
                .SelectMany(t => t.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static))
                .Where(f => f.IsLiteral && f.FieldType == typeof(string))
                .Select(f => (string)f.GetValue(null))
                .ToList();

            var model = new PermissionViewModel
            {
                RoleId = roleId,
                RoleName = role.Name,
                RoleClaims = allPermissions.Select(permission => new RoleClaimViewModel
                {
                    Type = "Permission",
                    Value = permission,
                    IsSelected = roleClaims.Any(c => c.Type == "Permission" && c.Value == permission)
                }).ToList()
            };

            return View(model);
        }

        // POST: Admin/Roles/ManagePermissions
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManagePermissions(PermissionViewModel model)
        {
            var role = await _roleManager.FindByIdAsync(model.RoleId);
            if (role == null)
            {
                return NotFound();
            }

            var currentClaims = await _roleManager.GetClaimsAsync(role);

            // حذف مجوزهای قبلی
            foreach (var claim in currentClaims.Where(c => c.Type == "Permission"))
            {
                await _roleManager.RemoveClaimAsync(role, claim);
            }

            // افزودن مجوزهای جدید انتخاب شده
            foreach (var roleClaim in model.RoleClaims.Where(rc => rc.IsSelected))
            {
                await _roleManager.AddClaimAsync(role, new System.Security.Claims.Claim("Permission", roleClaim.Value));
            }

            // --- این بخش جدید و بسیار مهم است ---
            // به‌روزرسانی Security Stamp تمام کاربران عضو این نقش
            var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name);
            foreach (var user in usersInRole)
            {
                await _userManager.UpdateSecurityStampAsync(user);
            }
            // --- پایان بخش جدید ---

            TempData["SuccessMessage"] = "مجوزها با موفقیت به‌روزرسانی شدند.";
            return RedirectToAction(nameof(Index));
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