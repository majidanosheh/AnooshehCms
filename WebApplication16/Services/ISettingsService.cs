using WebApplication16.ViewModels;

namespace WebApplication16.Services
{
    public interface ISettingsService
    {
        SiteSettings GetSettings();
        void ClearCache(); // متدی برای ابطال کش
    }
}
