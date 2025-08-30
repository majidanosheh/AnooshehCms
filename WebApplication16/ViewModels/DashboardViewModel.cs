using WebApplication16.ViewModels;

namespace WebApplication16.ViewModels
{
    /// <summary>
    /// این ViewModel به صورت اختصاصی برای ویوی داشبورد طراحی شده است.
    /// وظیفه آن، نگهداری تمام داده‌های مورد نیاز برای نمایش در داشبورد به صورت یک شیء ساختاریافته است.
    /// </summary>
    public class DashboardViewModel
    {
        // بخش کارت‌های آماری
        public int PagesCount { get; set; }
        public int PostsCount { get; set; }
        public int UsersCount { get; set; }
        public int MenusCount { get; set; }

        // بخش نمودار میله‌ای (مقالات اخیر)
        public List<string> BarChartLabels { get; set; } = new List<string>();
        public List<int> BarChartData { get; set; } = new List<int>();

        // بخش نمودار دونات (توزیع محتوا)
        public List<string> DoughnutChartLabels { get; set; } = new List<string> { "صفحات", "مقالات" };
        public List<int> DoughnutChartData { get; set; } = new List<int>();

        // بخش ویجت آخرین فعالیت‌ها
        public List<AuditLogViewModel> RecentActivities { get; set; } = new List<AuditLogViewModel>();
    }
}
