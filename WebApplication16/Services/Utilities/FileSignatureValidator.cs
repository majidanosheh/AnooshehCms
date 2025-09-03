using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WebApplication16.Services
{
    public static class FileSignatureValidator
    {
        // لیستی از امضاهای فایل‌های تصویری مجاز
        private static readonly Dictionary<string, List<byte[]>> _fileSignatures = new Dictionary<string, List<byte[]>>
        {
            { ".gif", new List<byte[]> { new byte[] { 0x47, 0x49, 0x46, 0x38 } } },
            { ".png", new List<byte[]> { new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A } } },
            { ".jpeg", new List<byte[]>
                {
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE1 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE8 }
                }
            },
            { ".jpg", new List<byte[]>
                {
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE1 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE8 }
                }
            }
        };

        public static bool IsValidImage(Stream fileStream, string fileName)
        {
            if (fileStream == null || fileStream.Length == 0)
            {
                return false;
            }

            var ext = Path.GetExtension(fileName).ToLowerInvariant();
            if (string.IsNullOrEmpty(ext) || !_fileSignatures.ContainsKey(ext))
            {
                return false;
            }

            fileStream.Position = 0;
            var reader = new BinaryReader(fileStream);
            var signatures = _fileSignatures[ext];
            var headerBytes = reader.ReadBytes(signatures.Max(m => m.Length));

            // بازگرداندن stream به موقعیت اولیه برای استفاده‌های بعدی
            fileStream.Position = 0;

            return signatures.Any(signature => headerBytes.Take(signature.Length).SequenceEqual(signature));
        }
    }
}