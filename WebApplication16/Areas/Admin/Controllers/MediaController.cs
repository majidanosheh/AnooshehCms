using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication16.Services;
using System.IO;
using System.Threading.Tasks;
using System;

namespace WebApplication16.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    [ApiController]
    public class MediaController : ControllerBase
    {
        private readonly IFileStorageService _fileStorageService;

        public MediaController(IFileStorageService fileStorageService)
        {
            _fileStorageService = fileStorageService;
        }

        [HttpPost("/api/media/upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new { message = "No file uploaded." });
            }

            try
            {
                // **اصلاح نهایی:** استفاده از StoreFileAsync با Stream برای سازگاری با کنترلرهای قدیمی
                using (var stream = file.OpenReadStream())
                {
                    // "uploads/media" مسیر ذخیره سازی پیش فرض در سرویس LocalFileStorageService است
                    var fileUrl = await _fileStorageService.StoreFileAsync(stream, file.FileName, "uploads/media");
                    return Ok(new { location = fileUrl });
                }
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // logging...
                return StatusCode(500, new { message = $"Internal server error: {ex.Message}" });
            }
        }
    }
}

//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using WebApplication16.Services;
//using System;
//using System.Threading.Tasks;

//namespace WebApplication16.Areas.Admin.Controllers
//{
//    [Area("Admin")]
//    [Authorize]
//    [ApiController]
//    public class MediaController : ControllerBase
//    {
//        private readonly IFileStorageService _fileStorageService;

//        public MediaController(IFileStorageService fileStorageService)
//        {
//            _fileStorageService = fileStorageService;
//        }

//        /// <summary>
//        /// آپلود فایل و ذخیره آن در سرویس ذخیره‌سازی محلی.
//        /// این API معمولاً توسط ویرایشگرهای متن (مانند TinyMCE) استفاده می‌شود.
//        /// </summary>
//        /// <param name="file">فایل ارسالی (به صورت IFormFile)</param>
//        /// <returns>آدرس URL فایل آپلود شده.</returns>
//        [HttpPost("/api/media/upload")]
//        [DisableRequestSizeLimit] // اجازه آپلود فایل‌های حجیم
//        public async Task<IActionResult> Upload(IFormFile file)
//        {
//            if (file == null || file.Length == 0)
//            {
//                return BadRequest(new { message = "فایلی برای آپلود ارسال نشده است." });
//            }

//            try
//            {
//                // FIX: استفاده از متد StoreFileAsync که در اینترفیس تعریف شده است.
//                // مسیر ذخیره سازی به صورت پیش فرض: wwwroot/form_uploads
//                var fileUrl = await _fileStorageService.StoreFileAsync(file, "uploads/media");

//                if (string.IsNullOrEmpty(fileUrl))
//                {
//                    return StatusCode(500, new { message = "ذخیره سازی فایل ناموفق بود." });
//                }

//                // فرمت بازگشتی برای ویرایشگرهای متن (مانند TinyMCE)
//                return Ok(new { location = fileUrl });
//            }
//            catch (ArgumentException ex)
//            {
//                // خطاهای مربوط به اعتبارسنجی نوع فایل یا سایر خطاها
//                return BadRequest(new { message = ex.Message });
//            }
//            catch (Exception ex)
//            {
//                // TODO: در اینجا باید از سیستم لاگینگ استفاده شود
//                return StatusCode(500, new { message = $"خطای داخلی سرور: {ex.Message}" });
//            }
//        }
//    }
//}
