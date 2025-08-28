using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication16.Areas.Identity.DataAccess;


namespace YourCmsName.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize] // تضمین می‌کند که فقط کاربران لاگین کرده می‌توانند به این API دسترسی داشته باشند
    [ApiController] // این اتریبیوت، رفتارهای استاندارد یک API (مانند بازگرداندن خودکار خطای 400) را فعال می‌کند
    public class MenuApiController : ControllerBase
    {
        private readonly WebApplication16Context _context;

        public MenuApiController(WebApplication16Context context)
        {
            _context = context;
        }

        // توضیح معماری: ما یک مسیر ثابت و مشخص برای این API تعریف می‌کنیم.
        // این کار باعث می‌شود که منطق فرانت‌اند ما به ساختار مسیریابی MVC وابسته نباشد.
        [HttpPost("/api/menus/update-order")]
        // توضیح امنیتی: ما باید حتماً توکن ضدجعل را در API های POST که توسط مرورگر فراخوانی می‌شوند، اعتبارسنجی کنیم.
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateOrder([FromBody] List<int> orderedIds)
        {
            if (orderedIds == null || !orderedIds.Any())
            {
                return BadRequest("لیست شناسه‌ها نمی‌تواند خالی باشد.");
            }

            // توضیح بهینه‌سازی: به جای خواندن تک تک آیتم‌ها در یک حلقه، ما تمام آیتم‌های مورد نیاز را
            // تنها با یک کوئری بهینه از پایگاه داده می‌خوانیم. این کار عملکرد را به شدت بهبود می‌بخشد.
            var menuItems = await _context.MenuItems
                                          .Where(mi => orderedIds.Contains(mi.Id))
                                          .ToListAsync();

            // این حلقه، ترتیب جدید را بر اساس موقعیت هر شناسه در لیست دریافتی، به‌روزرسانی می‌کند.
            for (int i = 0; i < orderedIds.Count; i++)
            {
                var itemId = orderedIds[i];
                var menuItem = menuItems.FirstOrDefault(mi => mi.Id == itemId);
                if (menuItem != null)
                {
                    menuItem.Order = i + 1; // ترتیب جدید از ۱ شروع می‌شود
                }
            }

            await _context.SaveChangesAsync();
            // توضیح UX: بازگرداندن یک پاسخ موفق (200 OK) به کلاینت اطلاع می‌دهد که عملیات با موفقیت انجام شده است.
            // در سناریوهای پیشرفته‌تر، می‌توانیم آیتم‌های به‌روز شده را نیز برگردانیم.
            return Ok(new { message = "ترتیب با موفقیت ذخیره شد." });
        }
    }
}