namespace WebApplication16.Services
{
    public class LocalFileStorageService : IFileStorageService
    {
        private readonly IWebHostEnvironment _env;
        private readonly IFileTypeValidator _fileTypeValidator; 
        private readonly IHttpContextAccessor _httpContextAccessor;


        public LocalFileStorageService(IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor, IFileTypeValidator fileTypeValidator)
        {
            _env = env;
            _httpContextAccessor = httpContextAccessor;
            _fileTypeValidator = fileTypeValidator; 

        }

        public async Task<string> SaveFileAsync(Stream fileStream, string fileName)
        {
            // 1. اعتبارسنجی امضای فایل
            if (!await _fileTypeValidator.IsValidFileAsync(fileStream, fileName))
            {
                throw new ArgumentException("فرمت فایل معتبر نیست، حجم آن زیاد است یا با پسوند آن مطابقت ندارد.");
            }

            // 2. محدودیت حجم (مثلاً ۵ مگابایت)
            const int maxFileSize = 5 * 1024 * 1024; // 5 MB
            if (fileStream.Length > maxFileSize)
            {
                throw new ArgumentException($"حجم فایل نباید بیشتر از {maxFileSize / 1024 / 1024} مگابایت باشد.");
            }

            var uploadsPath = Path.Combine(_env.WebRootPath, "uploads", "images");
            if (!Directory.Exists(uploadsPath))
            {
                Directory.CreateDirectory(uploadsPath);
            }

            var uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(fileName)}"; // استفاده از Path.GetExtension برای امنیت بیشتر
            var filePath = Path.Combine(uploadsPath, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await fileStream.CopyToAsync(stream);
            }

            var request = _httpContextAccessor.HttpContext.Request;
            var baseUrl = $"{request.Scheme}://{request.Host}";

            return $"{baseUrl}/uploads/images/{uniqueFileName}";
        }
    }
}

