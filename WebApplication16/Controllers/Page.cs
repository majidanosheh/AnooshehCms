using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication16.Areas.Identity.DataAccess;

namespace WebApplication16.Controllers 
{
    public class PageController : Controller
    {
        private readonly WebApplication16Context _context;

        public PageController(WebApplication16Context context)
        {
            _context = context;
        }

        // این اکشن هر آدرسی را دریافت خواهد کرد
        public async Task<IActionResult> Display(string slug)
        {
            if (string.IsNullOrWhiteSpace(slug))
            {
                // اگر اسلاگ خالی بود، به صفحه اصلی برود
                return RedirectToAction("Index", "Home");
            }

            // جستجو برای پیدا کردن صفحه با اسلاگ مورد نظر در پایگاه داده
            var page = await _context.Pages.FirstOrDefaultAsync(p => p.Slug == slug);

            // اگر صفحه‌ای با این آدرس پیدا نشد، خطای 404 نمایش داده شود
            if (page == null)
            {
                return NotFound();
            }

            // ارسال مدل Page به ویو برای نمایش
            return View(page);
        }
    }
}