using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Linq;

namespace WebApplication16.Services
{
    public class LocalFileStorageService : IFileStorageService
    {
        private readonly IWebHostEnvironment _env;
        private const string UploadsFolderName = "form_uploads";

        public LocalFileStorageService(IWebHostEnvironment env)
        {
            _env = env;
        }

        /// <summary>
        /// فایل را در مسیر wwwroot/form_uploads ذخیره می‌کند.
        /// </summary>
        /// <param name="file">فایل ارسالی</param>
        /// <param name="folderName">زیرپوشه برای ذخیره فایل (اختیاری)</param>
        /// <returns>مسیر نسبی فایل ذخیره شده</returns>
        public async Task<string?> StoreFileAsync(IFormFile file, string folderName = UploadsFolderName)
        {
            if (file == null || file.Length == 0)
            {
                return null;
            }

            // اطمینان از نام‌گذاری مناسب برای جلوگیری از تداخل
            var uploadsFolder = Path.Combine(_env.WebRootPath, folderName);
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // تضمین نام فایل منحصر به فرد
            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            // بازگرداندن مسیر نسبی برای ذخیره در دیتابیس
            return $"/{folderName}/{uniqueFileName}";
        }

        public void DeleteFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return;
            }

            var fullPath = Path.Combine(_env.WebRootPath, filePath.TrimStart('/'));
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }
    }
}
