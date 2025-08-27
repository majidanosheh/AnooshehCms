using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication16.Areas.Identity.DataAccess;
using WebApplication16.Models;


namespace WebApplication16.Controllers
{
    public class BlogController : Controller
    {
        private readonly WebApplication16Context _context;
        private const int PageSize = 5; // تعریف تعداد آیتم در هر صفحه به عنوان یک ثابت برای استفاده مجدد

        public BlogController(WebApplication16Context context)
        {
            _context = context;
        }

        // GET: /blog
        public async Task<IActionResult> Index(int pageNumber = 1)
        {
            // توضیح کوئری: این کوئری پایه برای نمایش تمام مقالات منتشر شده است.
            var postsQuery = _context.Posts
                .Where(p => p.IsPublished)
                .Include(p => p.Category) // بارگذاری حریصانه دسته‌بندی برای جلوگیری از N+1 Query
                .OrderByDescending(p => p.PublishDate);

            // توضیح معماری: ما منطق صفحه‌بندی را به یک متد جداگانه منتقل می‌کنیم
            // تا از تکرار کد در اکشن‌های دیگر جلوگیری کنیم (اصل DRY).
            await PaginateAndSetViewBag(postsQuery, pageNumber);

            var posts = await postsQuery
                .Skip((pageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            return View(posts);
        }

        // GET: /blog/{slug}
        [Route("blog/{slug}")]
        public async Task<IActionResult> Post(string slug)
        {
            if (string.IsNullOrEmpty(slug)) return NotFound();

            // توضیح عمیق‌تر Eager Loading:
            // .Include(p => p.PostTags).ThenInclude(pt => pt.Tag) یک مثال عالی از بارگذاری حریصانه تو در تو است.
            // این دستور به Entity Framework می‌گوید: "وقتی مقاله را بارگذاری می‌کنی، به جدول PostTags برو و رکوردهای مرتبط را پیدا کن،
            // و سپس برای هر کدام از آن رکوردها، به جدول Tags برو و تگ مرتبط را هم بارگذاری کن."
            // همه این کارها در یک کوئری SQL واحد انجام می‌شود که عملکرد را به شدت بهبود می‌بخشد.
            var post = await _context.Posts
                .Include(p => p.Category)
                .Include(p => p.PostTags)
                    .ThenInclude(pt => pt.Tag)
                .FirstOrDefaultAsync(p => p.Slug == slug && p.IsPublished);

            if (post == null) return NotFound();

            return View(post);
        }

        // GET: /blog/category/{categorySlug}
        [Route("blog/category/{categorySlug}")]
        public async Task<IActionResult> Category(string categorySlug, int pageNumber = 1)
        {
            if (string.IsNullOrEmpty(categorySlug)) return BadRequest();

            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Slug == categorySlug);
            if (category == null) return NotFound();

            var postsQuery = _context.Posts
                .Where(p => p.IsPublished && p.CategoryId == category.Id)
                .Include(p => p.Category)
                .OrderByDescending(p => p.PublishDate);

            await PaginateAndSetViewBag(postsQuery, pageNumber, $"مقالات در دسته‌بندی: {category.Name}", "Category", category.Slug);

            var posts = await postsQuery.Skip((pageNumber - 1) * PageSize).Take(PageSize).ToListAsync();

            // توضیح معماری: ما از همان ویوی Index برای نمایش لیست مقالات استفاده می‌کنیم.
            // این یک مثال از اصل "Don't Repeat Yourself" (DRY) است. به جای ساختن یک ویو جداگانه برای آرشیو دسته‌بندی،
            // ما ویوی Index را هوشمندتر می‌کنیم تا بتواند انواع مختلف لیست‌ها را نمایش دهد.
            return View("Index", posts);
        }

        // GET: /blog/tag/{tagSlug}
        [Route("blog/tag/{tagSlug}")]
        public async Task<IActionResult> Tag(string tagSlug, int pageNumber = 1)
        {
            if (string.IsNullOrEmpty(tagSlug)) return BadRequest();

            var tag = await _context.Tags.FirstOrDefaultAsync(t => t.Slug == tagSlug);
            if (tag == null) return NotFound();

            // توضیح کوئری چند-به-چند:
            // برای پیدا کردن مقالات یک تگ، ما باید از طریق جدول واسط PostTags عمل کنیم.
            // .Where(p => ... p.PostTags.Any(pt => pt.Tag.Slug == tagSlug))
            // این کوئری به زبان SQL به چیزی شبیه به "SELECT ... FROM Posts WHERE EXISTS (SELECT 1 FROM PostTags ... WHERE Tag.Slug = ...)" ترجمه می‌شود.
            var postsQuery = _context.Posts
                .Where(p => p.IsPublished && p.PostTags.Any(pt => pt.Tag.Slug == tagSlug))
                .Include(p => p.Category)
                .OrderByDescending(p => p.PublishDate);

            await PaginateAndSetViewBag(postsQuery, pageNumber, $"مقالات با تگ: {tag.Name}", "Tag", tag.Slug);

            var posts = await postsQuery.Skip((pageNumber - 1) * PageSize).Take(PageSize).ToListAsync();

            return View("Index", posts);
        }

        // متد کمکی برای جلوگیری از تکرار کد صفحه‌بندی
        private async Task PaginateAndSetViewBag(IQueryable<Post> postsQuery, int pageNumber, string? archiveTitle = null, string? archiveType = null, string? archiveSlug = null)
        {
            ViewBag.TotalPages = (int)Math.Ceiling(await postsQuery.CountAsync() / (double)PageSize);
            ViewBag.CurrentPage = pageNumber;
            ViewBag.ArchiveTitle = archiveTitle;
            ViewBag.ArchiveSlug = archiveSlug;
            ViewBag.ArchiveType = archiveType;
        }
    }
}