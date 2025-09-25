using System.Threading.Tasks;

namespace WebApplication16.Services
{
    public interface IShortcodeService
    {
        Task<string> ProcessShortcodesAsync(string content);
    }
}
