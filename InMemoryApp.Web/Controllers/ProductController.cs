using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace InMemoryApp.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IMemoryCache _memoryCache;
        public ProductController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
        public IActionResult Index()
        {
            // check cache if already exist
            //if (String.IsNullOrEmpty(_memoryCache.Get<string>("time")))
            //    _memoryCache.Set<string>("time", DateTime.Now.ToString());
            //or
            //if (!_memoryCache.TryGetValue<string>("time", out string timecache))
            //{
            MemoryCacheEntryOptions opt = new MemoryCacheEntryOptions();
            opt.AbsoluteExpiration = DateTime.Now.AddMinutes(1);
            opt.SlidingExpiration = TimeSpan.FromSeconds(10);
            opt.Priority = CacheItemPriority.NeverRemove; //not removes from memory even its full. can be problematic if memory getting full if you set all caches NeverRemove
            _memoryCache.Set<string>("time", DateTime.Now.ToString());
            //}

            return View();
        }

        public IActionResult ShowTime()
        {
            //can get if exist otherwise creates new
            //_memoryCache.GetOrCreate<string>("time", entry =>
            //{
            //    entry.AbsoluteExpiration = DateTime.Now.AddMinutes(5);
            //    return DateTime.Now.ToString();
            //});
            //or
            _memoryCache.TryGetValue("time", out string timecache);
            ViewBag.Time = timecache;
            //ViewBag.Time = _memoryCache.Get<string>("time");
            return View();
        }
    }
}
