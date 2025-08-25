using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication16.Areas.Identity.DataAccess;

namespace WebApplication16.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly WebApplication16Context _context;
        private readonly UserManager<IdentityUser> _userManager;

        public DashboardController(WebApplication16Context context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.PagesCount = await _context.Pages.CountAsync();
            ViewBag.UsersCount = await _userManager.Users.CountAsync();
            // در آینده می‌توانیم آمارهای بیشتری مانند تعداد منوها، مقالات و... را نیز اضافه کنیم.
            return View();
        }
    }
}