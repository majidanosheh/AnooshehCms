using Microsoft.Extensions.Caching.Memory;
using WebApplication16.Areas.Identity.DataAccess;
using WebApplication16.Services;
using WebApplication16.ViewModels;


namespace WebApplication16.Services
{
    public class SettingsService : ISettingsService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IMemoryCache _cache;
        private const string SettingsCacheKey = "SiteSettings";

        public SettingsService(IServiceProvider serviceProvider, IMemoryCache cache)
        {
            _serviceProvider = serviceProvider;
            _cache = cache;
        }

        public SiteSettings GetSettings()
        {
            if (!_cache.TryGetValue(SettingsCacheKey, out SiteSettings settings))
            {
                // ایجاد یک scope جدید برای دریافت DbContext
                using (var scope = _serviceProvider.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<WebApplication16Context>();
                    var settingsFromDb = context.SiteSettings.ToDictionary(s => s.Key, s => s.Value);

                    settings = new SiteSettings
                    {
                        SiteName = settingsFromDb.GetValueOrDefault("SiteName"),
                        SiteDescription = settingsFromDb.GetValueOrDefault("SiteDescription")
                    };
                }

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromHours(1));

                _cache.Set(SettingsCacheKey, settings, cacheEntryOptions);
            }

            return settings;
        }

        public void ClearCache()
        {
            _cache.Remove(SettingsCacheKey);
        }
    }
}