using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication16.Areas.Identity.DataAccess;
using WebApplication16.Models;
using WebApplication16.ViewModels;
 // اطمینان از وجود using برای PaginatedList

namespace YourCmsName.Controllers
{
    public class BlogController : Controller
    {
        private readonly WebApplication16Context _context;
        private const int PageSize = 10; // تعریف تعداد مقالات در هر صفحه به عنوان یک ثابت برای خوانایی و مدیریت آسان

        public BlogController(WebApplication16Context context)
        {
            _context = context;
        }

        // توضیح معماری: این اکشن دو مسیر (Route) مختلف را مدیریت می‌کند.
        // این کار به ما اجازه می‌دهد تا هم صفحه اصلی وبلاگ (/blog) و هم صفحات بعدی آن (/blog/page/2) را با یک اکشن واحد مدیریت کنیم.
        [Route("blog")]
        [Route("blog/page/{pageNumber:int}")]
        public async Task<IActionResult> Index(int pageNumber = 1)
        {
            // توضیح بهینه‌سازی کوئری:
            // ما کوئری را به صورت IQueryable می‌سازیم و هنوز آن را اجرا نکرده‌ایم.
            // این به ما اجازه می‌دهد تا منطق صفحه‌بندی را قبل از ارسال کوئری نهایی به دیتابیس، به آن اضافه کنیم.
            var postsQuery = _context.Posts
                .Include(p => p.Category) // Eager Loading: به EF Core می‌گوییم که اطلاعات دسته‌بندی را همزمان با مقالات بارگذاری کند تا از مشکل N+1 جلوگیری شود.
                .OrderByDescending(p => p.CreatedAt);

            // در اینجا، کلاس کمکی PaginatedList کوئری را دریافت کرده و منطق Skip و Take را به آن اضافه می‌کند و سپس آن را اجرا می‌کند.
            var paginatedPosts = await PaginatedList<Post>.CreateAsync(postsQuery, pageNumber, PageSize);

            return View(paginatedPosts);
        }

        // توضیح معماری: این اکشن از Attribute Routing برای تعریف یک URL خوانا و بهینه برای SEO استفاده می‌کند.
        [Route("blog/{slug}")]
        public async Task<IActionResult> Details(string slug)
        {
            if (string.IsNullOrEmpty(slug))
            {
                return NotFound();
            }

            // توضیح بهینه‌سازی کوئری:
            // ما از .Include() و .ThenInclude() برای بارگذاری همزمان تمام داده‌های مرتبط (دسته‌بندی و تگ‌ها) در یک کوئری واحد استفاده می‌کنیم.
            // این کار از ارسال چندین کوئری جداگانه به دیتابیس جلوگیری کرده و عملکرد را به شدت بهبود می‌بخشد.
            var post = await _context.Posts
                .Include(p => p.Category)
                .Include(p => p.PostTags)
                    .ThenInclude(pt => pt.Tag) // بارگذاری تگ‌های مرتبط با هر PostTag
                .FirstOrDefaultAsync(m => m.Slug == slug);

            if (post == null)
            {
                // اگر مقاله‌ای با این اسلاگ پیدا نشد، یک صفحه 404 استاندارد نمایش می‌دهیم.
                return NotFound();
            }
            ViewData["MetaTitle"] = !string.IsNullOrEmpty(post.MetaTitle) ? post.MetaTitle : post.Title;
            ViewData["MetaDescription"] = post.MetaDescription;
            return View(post);
        }

        // توضیح معماری: این اکشن و اکشن بعدی (Tag)، یک الگوی طراحی هوشمند را نشان می‌دهند: "استفاده مجدد از ویو".
        // هر دو اکشن، داده‌های متفاوتی را آماده می‌کنند اما در نهایت از همان ویوی Index.cshtml برای نمایش لیست مقالات استفاده می‌کنند.
        [Route("blog/category/{slug}")]
        public async Task<IActionResult> Category(string slug, int pageNumber = 1)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Slug == slug);
            if (category == null) return NotFound();

            var postsQuery = _context.Posts
                .Where(p => p.CategoryId == category.Id)
                .Include(p => p.Category)
                .OrderByDescending(p => p.CreatedAt);

            // ما از ViewData برای ارسال یک عنوان سفارشی به ویو استفاده می‌کنیم.
            ViewData["ArchiveTitle"] = $"آرشیو دسته‌بندی: {category.Name}";
            var paginatedPosts = await PaginatedList<Post>.CreateAsync(postsQuery, pageNumber, PageSize);

            return View("Index", paginatedPosts);
        }

        [Route("blog/tag/{slug}")]
        public async Task<IActionResult> Tag(string slug, int pageNumber = 1)
        {
            var tag = await _context.Tags.FirstOrDefaultAsync(t => t.Slug == slug);
            if (tag == null) return NotFound();

            // توضیح کوئری پیچیده:
            // این کوئری LINQ، تمام مقالاتی را پیدا می‌کند که در جدول واسط PostTags، حداقل یک رکورد مرتبط با تگ مورد نظر ما را داشته باشند.
            var postsQuery = _context.Posts
                .Where(p => p.PostTags.Any(pt => pt.Tag.Slug == slug))
                .Include(p => p.Category)
                .OrderByDescending(p => p.CreatedAt);

            ViewData["ArchiveTitle"] = $"آرشیو برچسب: {tag.Name}";
            var paginatedPosts = await PaginatedList<Post>.CreateAsync(postsQuery, pageNumber, PageSize);

            return View("Index", paginatedPosts);
        }
    }
}