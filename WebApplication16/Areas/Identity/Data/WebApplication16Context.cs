using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using YourCmsName.Models;

namespace WebApplication16.Areas.Identity.DataAccess
{
    public class WebApplication16Context : IdentityDbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;


        public WebApplication16Context(DbContextOptions<WebApplication16Context> options, IHttpContextAccessor httpContextAccessor)
        : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public DbSet<Page> Pages { get; set; }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is BaseEntity && (
                        e.State == EntityState.Added ||
                        e.State == EntityState.Modified));

            // گرفتن شناسه کاربر فعلی
            //var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

            foreach (var entityEntry in entries)
            {
                var entity = (BaseEntity)entityEntry.Entity;
                entity.ModifiedAt = DateTime.UtcNow;
                //entity.ModifiedBy = userId;

                if (entityEntry.State == EntityState.Added)
                {
                    entity.CreatedAt = DateTime.UtcNow;
                    //entity.CreatedBy = userId;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
