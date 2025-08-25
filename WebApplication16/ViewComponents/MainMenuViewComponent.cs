using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication16.Areas.Identity.DataAccess;

namespace WebApplication16.ViewComponents
{
    // نام کلاس باید به "ViewComponent" ختم شود تا ASP.NET Core آن را شناسایی کند.
    public class MainMenuViewComponent : ViewComponent
    {
        private readonly WebApplication16Context _context;

        // توضیح معماری: ما DbContext را از طریق تزریق وابستگی دریافت می‌کنیم.
        // این کار باعث می‌شود که View Component ما به صورت مستقل و تست‌پذیر باقی بماند.
        public MainMenuViewComponent(WebApplication16Context context)
        {
            _context = context;
        }

        // متد InvokeAsync قلب هر View Component است.
        // این متد زمانی اجرا می‌شود که کامپوننت در یک ویو فراخوانی شود.
        public async Task<IViewComponentResult> InvokeAsync()
        {
            // توضیح معماری: ما منویی با نام مشخص "MainMenu" را به همراه تمام آیتم‌ها و زیرآیتم‌هایش بارگذاری می‌کنیم.
            // استفاده از Include و ThenInclude برای بارگذاری حریصانه (Eager Loading) حیاتی است.
            // بدون این کار، با مشکل "N+1 Query" مواجه می‌شویم که در آن برای هر آیتم منو، یک کوئری جداگانه به دیتابیس زده می‌شود.
            var menu = await _context.Menus
                .Include(m => m.MenuItems)
                    .ThenInclude(mi => mi.SubMenuItems)
                .FirstOrDefaultAsync(m => m.Name == "MainMenu");

            if (menu == null)
            {
                // توضیح UX: اگر مدیر سایت هنوز منویی به نام "MainMenu" نساخته باشد،
                // ما یک خطای بزرگ نمایش نمی‌دهیم، بلکه به سادگی یک بخش خالی را رندر می‌کنیم.
                // این یک روش "دفاعی" و کاربرپسند برای مدیریت داده‌های اختیاری است.
                return Content(string.Empty);
            }

            // ما فقط آیتم‌های سطح بالا (آنهایی که والد ندارند) را به ویو ارسال می‌کنیم.
            // زیرآیتم‌های هر آیتم، از طریق ویژگی ناوبری SubMenuItems که قبلاً بارگذاری کرده‌ایم، در خود ویو قابل دسترسی خواهند بود.
            var topLevelItems = menu.MenuItems
                                    .Where(mi => mi.ParentMenuItemId == null)
                                    .OrderBy(mi => mi.Order)
                                    .ToList();

            return View(topLevelItems); // ارسال مدل به ویو کامپوننت
        }
    }
}