using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplication16.Areas.Identity.DataAccess;
using WebApplication16.Constants;
using WebApplication16.Services;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("WebApplication16ContextConnection") ?? throw new InvalidOperationException("Connection string 'WebApplication16ContextConnection' not found.");

builder.Services.AddDbContext<WebApplication16Context>(options => options.UseSqlServer(connectionString));

//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<WebApplication16Context>();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>() // <<-- این خط برای حل مشکل اضافه شد
    .AddEntityFrameworkStores<WebApplication16Context>();

builder.Services.AddScoped<IFileStorageService, LocalFileStorageService>();

builder.Services.AddAuthorization(options =>
{
    // توضیح معماری: این کد با استفاده از Reflection، به صورت پویا تمام ثابت‌های رشته‌ای
    // تعریف شده در کلاس Permissions و کلاس‌های تو در توی آن را پیدا می‌کند.
    // این یعنی اگر در آینده یک مجوز جدید به کلاس Permissions اضافه کنید،
    // نیازی به تغییر این بخش از کد نخواهید داشت و Policy مربوط به آن به صورت خودکار ساخته می‌شود.
    var permissions = typeof(Permissions).GetNestedTypes()
        .SelectMany(t => t.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static))
        .Where(f => f.IsLiteral && f.FieldType == typeof(string))
        .Select(f => (string)f.GetValue(null))
        .ToList();

    foreach (var permission in permissions)
    {
        // برای هر رشته مجوز، یک Policy با همان نام می‌سازیم.
        // این Policy فقط یک قانون دارد: کاربر باید یک Claim از نوع "Permission"
        // با مقداری برابر با نام مجوز داشته باشد.
        options.AddPolicy(permission, policy => policy.RequireClaim("Permission", permission));
    }
});


// Add services to the container.
builder.Services.AddControllersWithViews();
//
builder.Services.AddHttpContextAccessor();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.MapRazorPages();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.MapControllerRoute(
    name: "AdminArea",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
    );

app.MapControllerRoute(
    name: "Page",
    pattern: "{slug}", // هر آدرسی را به عنوان slug در نظر می‌گیرد
    defaults: new { controller = "Page", action = "Display" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
