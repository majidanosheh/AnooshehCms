namespace WebApplication16.Constants
{
    public static class Permissions
    {
        // توضیح معماری: ما از کلاس‌های استاتیک تو در تو برای گروه‌بندی مجوزها بر اساس ماژول استفاده می‌کنیم.
        // این کار باعث می‌شود که در زمان استفاده، کد ما بسیار خوانا باشد (مثلاً Permissions.Pages.Create).
        public static class Pages
        {
            public const string View = "Permissions.Pages.View";
            public const string Create = "Permissions.Pages.Create";
            public const string Edit = "Permissions.Pages.Edit";
            public const string Delete = "Permissions.Pages.Delete";
        }

        public static class Menus
        {
            public const string View = "Permissions.Menus.View";
            public const string Create = "Permissions.Menus.Create";
            public const string Edit = "Permissions.Menus.Edit";
            public const string Delete = "Permissions.Menus.Delete";
        }

        // پیامد: با رشد پروژه، افزودن ماژول‌ها و مجوزهای جدید بسیار ساده خواهد بود.
        // برای مثال، در آینده می‌توانیم به راحتی بخش کاربران را اضافه کنیم:
       
        public static class Users
        {
            public const string View = "Permissions.Users.View";
            public const string ManageRoles = "Permissions.Users.ManageRoles";
            // پیامد: در آینده می‌توانیم مجوزهای دقیق‌تری اضافه کنیم، مانند:
            // public const string Edit = "Permissions.Users.Edit";
            // public const string Delete = "Permissions.Users.Delete";
        }
        public static class AuditLogs
        {
            public const string View = "Permissions.AuditLogs.View";
        }
        public static class Settings
        {
            public const string Manage = "Permissions.Settings.Manage";
        }

        public static class Blog
        {
            public const string ViewPosts = "Permissions.Blog.ViewPosts";
            public const string CreatePosts = "Permissions.Blog.CreatePosts";
            public const string EditPosts = "Permissions.Blog.EditPosts";
            public const string DeletePosts = "Permissions.Blog.DeletePosts";
            public const string ManageCategories = "Permissions.Blog.ManageCategories";
            public const string ManageTags = "Permissions.Blog.ManageTags";
        }

        public static class Roles
        {
            public const string View = "Permissions.Roles.View";
            public const string Create = "Permissions.Roles.Create";
            public const string Edit = "Permissions.Roles.Edit";
            public const string Delete = "Permissions.Roles.Delete";
        }

        public static class Tickets
        {
            public const string View = "Permissions.Tickets.View";
            public const string Create = "Permissions.Roles.Create";
            public const string Edit = "Permissions.Roles.Edit";
            public const string Delete = "Permissions.Roles.Delete";
        }
    }
}