using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using WebApplication16.Areas.Identity.DataAccess;
using WebApplication16.Models;
using WebApplication16.Services;



public class DbFileTypeValidator : IFileTypeValidator
{
    private readonly WebApplication16Context _context;
    private readonly IMemoryCache _cache;
    private const string AllowedTypesCacheKey = "AllowedFileTypes";

    public DbFileTypeValidator(WebApplication16Context context, IMemoryCache cache)
    {
        _context = context;
        _cache = cache;
    }

    public async Task<bool> IsValidFileAsync(Stream fileStream, string fileName)
    {
        if (fileStream == null || fileStream.Length == 0) return false;

        var allowedTypes = await GetAllowedFileTypesAsync();
        var ext = Path.GetExtension(fileName).ToLowerInvariant();

        var allowedType = allowedTypes.FirstOrDefault(t => t.Extension == ext);
        if (allowedType == null) return false; // پسوند مجاز نیست

        if (fileStream.Length > allowedType.MaxSizeInBytes) return false; // حجم فایل زیاد است

        // منطق بررسی امضا
        fileStream.Position = 0;
        var reader = new BinaryReader(fileStream);
        var signatures = allowedType.Signatures.Split(',').Select(s => ConvertFromHex(s.Trim())).ToList();
        var headerBytes = reader.ReadBytes(signatures.Max(m => m.Length));
        fileStream.Position = 0;

        return signatures.Any(signature => headerBytes.Take(signature.Length).SequenceEqual(signature));
    }

    private async Task<List<AllowedFileType>> GetAllowedFileTypesAsync()
    {
        return await _cache.GetOrCreateAsync(AllowedTypesCacheKey, async entry =>
        {
            entry.SlidingExpiration = TimeSpan.FromHours(1);
            return await _context.allowedFileTypes.AsNoTracking().ToListAsync();
        });
    }

    private static byte[] ConvertFromHex(string hexString)
    {
        return Enumerable.Range(0, hexString.Length)
                         .Where(x => x % 2 == 0)
                         .Select(x => Convert.ToByte(hexString.Substring(x, 2), 16))
                         .ToArray();
    }
}

