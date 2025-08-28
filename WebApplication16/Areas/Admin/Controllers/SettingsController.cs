using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication16.Areas.Identity.DataAccess;
using WebApplication16.Models;
using WebApplication16.Services;
using WebApplication16.ViewModels;


namespace YourCmsName.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize] // در آینده می‌توانیم یک Policy خاص برای این بخش تعریف کنیم
    public class SettingsController : Controller
    {

        private readonly WebApplication16Context _context;
        private readonly ISettingsService _settingsService; // تزریق سرویس
        public SettingsController(WebApplication16Context context, ISettingsService settingsService)
        {
            _context = context;
            _settingsService = settingsService; // مقداردهی سرویس
        }

       

        // GET: Admin/Settings
        public async Task<IActionResult> Index()
        {
            // توضیح بهینه‌سازی: ما با استفاده از ToDictionaryAsync، تمام تنظیمات را تنها با یک کوئری
            // از پایگاه داده خوانده و در یک ساختار داده‌ای سریع (دیکشنری) برای دسترسی آسان قرار می‌دهیم.
            var settings = await _context.SiteSettings.ToDictionaryAsync(s => s.Key, s => s.Value);

            // توضیح خوانایی: GetValueOrDefault یک روش امن برای خواندن از دیکشنری است.
            // اگر کلید مورد نظر (مثلاً "SiteName") وجود نداشته باشد، به جای ایجاد خطا، مقدار null را برمی‌گرداند.
            var viewModel = new SettingsViewModel
            {
                SiteName = settings.GetValueOrDefault("SiteName"),
                SiteDescription = settings.GetValueOrDefault("SiteDescription")
            };
            return View(viewModel);
        }

        // POST: Admin/Settings
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(SettingsViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                await UpdateSettingAsync("SiteName", viewModel.SiteName);
                await UpdateSettingAsync("SiteDescription", viewModel.SiteDescription);

                // پیامد: با فراخوانی این متد، ما تضمین می‌کنیم که درخواست بعدی برای تنظیمات،
                // حتماً از پایگاه داده خوانده شده و نسخه جدید و به‌روز در کش قرار می‌گیرد.
                _settingsService.ClearCache(); // <<-- ابطال کش

                TempData["SuccessMessage"] = "تنظیمات با موفقیت ذخیره شد.";
                return RedirectToAction(nameof(Index));
            }
            return View(viewModel);
        }

        // این متد کمکی، الگوی "Upsert" (Update or Insert) را پیاده‌سازی می‌کند.
        private async Task UpdateSettingAsync(string key, string? value)
        {
            var setting = await _context.SiteSettings.FindAsync(key);
            if (setting == null)
            {
                // اگر تنظیمات وجود نداشت، آن را ایجاد کن (Insert)
                _context.SiteSettings.Add(new SiteSetting { Key = key, Value = value });
            }
            else
            {
                // اگر وجود داشت، آن را به‌روزرسانی کن (Update)
                setting.Value = value;
            }
            // پیامد: این متد در هر فراخوانی، یک بار SaveChangesAsync را اجرا می‌کند.
            // برای تعداد زیادی تنظیمات، بهتر است تمام تغییرات را در DbContext اعمال کرده
            // و در نهایت، فقط یک بار SaveChangesAsync را در اکشن POST فراخوانی کنیم.
            await _context.SaveChangesAsync();
        }
    }
}
