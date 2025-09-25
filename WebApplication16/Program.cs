using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplication16.Areas.Identity.DataAccess;
using WebApplication16.Constants;
using WebApplication16.Services;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("WebApplication16ContextConnection") ?? throw new InvalidOperationException("Connection string 'WebApplication16ContextConnection' not found.");

builder.Services.AddDbContext<WebApplication16Context>(options => options.UseSqlServer(connectionString));
builder.Services.AddMemoryCache();

builder.Services.AddSingleton<ISettingsService, SettingsService>();
builder.Services.AddScoped<INotificationService, EmailNotificationService>();

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<WebApplication16Context>();

builder.Services.Configure<SecurityStampValidatorOptions>(options =>
{
    options.ValidationInterval = TimeSpan.FromMinutes(1);
});
builder.Services.AddScoped<IMenuService, MenuService>();
builder.Services.AddScoped<IFileStorageService, LocalFileStorageService>();

// --- ثبت سرویس های جدید ---
builder.Services.AddScoped<IRazorViewToStringRenderer, RazorViewToStringRenderer>();
builder.Services.AddScoped<IShortcodeService, ShortcodeService>();
// -------------------------

builder.Services.AddAuthorization(options =>
{
    var permissions = typeof(Permissions).GetNestedTypes()
        .SelectMany(t => t.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static))
        .Where(f => f.IsLiteral && f.FieldType == typeof(string))
        .Select(f => (string)f.GetValue(null)!)
        .ToList();

    foreach (var permission in permissions)
    {
        options.AddPolicy(permission, policy => policy.RequireClaim("Permission", permission));
    }
});

builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IFileTypeValidator, DbFileTypeValidator>();
builder.Services.AddHttpContextAccessor();

// --- ثبت سرویس های فرم ساز ---
builder.Services.AddScoped<IFormService, FormService>();
builder.Services.AddScoped<IFormFieldService, FormFieldService>();
builder.Services.AddScoped<ISubmissionService, SubmissionService>();
// ---------------------------

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.MapRazorPages();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "AdminArea",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "Blog",
    pattern: "blog/{action=Index}/{slug?}",
    defaults: new { controller = "Blog" });

app.MapControllerRoute(
    name: "FormRenderer",
    pattern: "form/{slug}",
    defaults: new { controller = "FormRenderer", action = "Display" });


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "Page",
    pattern: "{slug}",
    defaults: new { controller = "Page", action = "Display" });


app.Run();


//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using WebApplication16.Areas.Identity.DataAccess;
//using WebApplication16.Constants;
//using WebApplication16.Services;; // Namespace for services

//var builder = WebApplication.CreateBuilder(args);
//var connectionString = builder.Configuration.GetConnectionString("WebApplication16ContextConnection") ?? throw new InvalidOperationException("Connection string 'WebApplication16ContextConnection' not found.");

//builder.Services.AddDbContext<WebApplication16Context>(options => options.UseSqlServer(connectionString));

//builder.Services.AddMemoryCache();

//// Existing Service Registrations
//builder.Services.AddSingleton<ISettingsService, SettingsService>();
//builder.Services.AddScoped<INotificationService, EmailNotificationService>();
//builder.Services.AddScoped<IMenuService, MenuService>();
//builder.Services.AddScoped<IFileStorageService, LocalFileStorageService>();
//builder.Services.AddScoped<IFileTypeValidator, DbFileTypeValidator>();

//// --- NEW: Registering Form Builder Services ---
//builder.Services.AddScoped<IFormService, FormService>();
//builder.Services.AddScoped<IFormFieldService, FormFieldService>();
//builder.Services.AddScoped<ISubmissionService, SubmissionService>();
//// -------------------------------------------

//builder.Services.AddDefaultIdentity<IdentityUser>(options =>
//{
//    options.SignIn.RequireConfirmedAccount = true;
//})
//.AddRoles<IdentityRole>()
//.AddEntityFrameworkStores<WebApplication16Context>();

//builder.Services.Configure<SecurityStampValidatorOptions>(options =>
//{
//    options.ValidationInterval = TimeSpan.FromMinutes(1);
//});

//builder.Services.AddAuthorization(options =>
//{
//    var permissions = typeof(Permissions).GetNestedTypes()
//        .SelectMany(t => t.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static))
//        .Where(f => f.IsLiteral && f.FieldType == typeof(string))
//        .Select(f => (string)f.GetValue(null))
//        .ToList();

//    foreach (var permission in permissions)
//    {
//        options.AddPolicy(permission, policy => policy.RequireClaim("Permission", permission));
//    }
//});

//builder.Services.AddControllersWithViews();
//builder.Services.AddHttpContextAccessor();

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseDeveloperExceptionPage();
//}
//else
//{
//    app.UseExceptionHandler("/Home/Error");
//    app.UseHsts();
//}

//app.MapRazorPages();
//app.UseHttpsRedirection();
//app.UseStaticFiles();
//app.UseRouting();
//app.UseAuthorization();

//// ۱. مسیر Admin Area (بالاترین اولویت)
//app.MapControllerRoute(
//    name: "AdminArea",
//    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

//// ۲. مسیرهای مشخص وبلاگ
//app.MapControllerRoute(
//    name: "Blog",
//    pattern: "blog/{action=Index}/{slug?}",
//    defaults: new { controller = "Blog" });

//// ۳. مسیر فرم رندر کننده
//app.MapControllerRoute(
//    name: "FormRenderer",
//    pattern: "form/{slug}",
//    defaults: new { controller = "FormRenderer", action = "Display" });


//// ۴. مسیر پیش‌فرض (باید قبل از مسیر صفحات باشد)
//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");

//// ۵. مسیر داینامیک صفحات (پایین‌ترین اولویت)
//app.MapControllerRoute(
//    name: "Page",
//    pattern: "{slug}",
//    defaults: new { controller = "Page", action = "Display" });

//app.Run();

