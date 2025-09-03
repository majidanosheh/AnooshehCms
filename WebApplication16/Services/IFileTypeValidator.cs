

namespace WebApplication16.Services
{
    public interface IFileTypeValidator
    {
        Task<bool> IsValidFileAsync(Stream fileStream, string fileName);
    }
}
