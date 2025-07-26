using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplication16.Areas.Identity.DataAccess;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("WebApplication16ContextConnection") ?? throw new InvalidOperationException("Connection string 'WebApplication16ContextConnection' not found.");

builder.Services.AddDbContext<WebApplication16Context>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<WebApplication16Context>();

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
