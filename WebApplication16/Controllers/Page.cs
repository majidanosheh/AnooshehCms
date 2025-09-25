using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WebApplication16.Areas.Identity.DataAccess;
using WebApplication16.Services;

namespace WebApplication16.Controllers
{
    // Note: Assuming the class name from Page.cs is PageController
    public class PageController : Controller
    {
        private readonly WebApplication16Context _context;
        private readonly IShortcodeService _shortcodeService;

        public PageController(WebApplication16Context context, IShortcodeService shortcodeService)
        {
            _context = context;
            _shortcodeService = shortcodeService;
        }

        public async Task<IActionResult> Display(string slug)
        {
            if (string.IsNullOrEmpty(slug))
            {
                return NotFound();
            }

            var page = await _context.Pages.FirstOrDefaultAsync(p => p.Slug == slug);

            if (page == null)
            {
                return NotFound();
            }

            // Process the content for shortcodes before sending it to the view
            page.Content = await _shortcodeService.ProcessShortcodesAsync(page.Content);

            return View(page);
        }
    }
}


//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using WebApplication16.Areas.Identity.DataAccess;

//namespace WebApplication16.Controllers 
//{
//    public class PageController : Controller
//    {
//        private readonly WebApplication16Context _context;

//        public PageController(WebApplication16Context context)
//        {
//            _context = context;
//        }

//        // این اکشن هر آدرسی را دریافت خواهد کرد
//        public async Task<IActionResult> Display(string slug)
//        {
//            if (string.IsNullOrWhiteSpace(slug))
//            {
//                // اگر اسلاگ خالی بود، به صفحه اصلی برود
//                return RedirectToAction("Index", "Home");
//            }

//            // جستجو برای پیدا کردن صفحه با اسلاگ مورد نظر در پایگاه داده
//            var page = await _context.Pages.FirstOrDefaultAsync(p => p.Slug == slug);

//            // اگر صفحه‌ای با این آدرس پیدا نشد، خطای 404 نمایش داده شود
//            if (page == null)
//            {
//                return NotFound();
//            }
//            // اگر MetaTitle خالی بود، از عنوان اصلی صفحه استفاده کن
//            ViewData["MetaTitle"] = !string.IsNullOrEmpty(page.MetaTitle) ? page.MetaTitle : page.Title;
//            ViewData["MetaDescription"] = page.MetaDescription;
//            // ارسال مدل Page به ویو برای نمایش
//            return View(page);
//        }
//    }
//}