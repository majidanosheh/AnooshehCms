using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication16.Services;

namespace WebApplication16.Web.Areas.Admin.Controllers
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

        [HttpPost("/api/media/upload")] // تعریف یک آدرس ثابت برای API
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new { message = "No file uploaded." });
            }

            try
            {
                var fileUrl = await _fileStorageService.SaveFileAsync(file.OpenReadStream(), file.FileName);
                return Ok(new { location = fileUrl });
            }
            catch (ArgumentException ex)
            {
                // خطاهای مربوط به اعتبارسنجی را به کاربر نمایش می‌دهیم
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // خطاهای سرور را به صورت کلی مدیریت می‌کنیم
                return StatusCode(500, new { message = $"Internal server error: {ex.Message}" });
            }
        }
    }
}