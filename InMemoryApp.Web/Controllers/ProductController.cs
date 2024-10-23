using InMemoryApp.Web.Models;
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
            opt.AbsoluteExpiration = DateTime.Now.AddSeconds(5);
            //opt.SlidingExpiration = TimeSpan.FromSeconds(10);
            opt.Priority = CacheItemPriority.High; //not removes from memory even its full. can be problematic if memory getting full if you set all caches NeverRemove
            opt.RegisterPostEvictionCallback((key, value, reason, state) =>
            {
                _memoryCache.Set("callback", $"{key}->{value} => reason:{reason}");
            });
            _memoryCache.Set<string>("time", DateTime.Now.ToString(), opt);
            //}

            Product p = new() { Id = 1, Name = "Pen", Price = 12 }; 
            _memoryCache.Set<Product>("product:1", p);

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
            _memoryCache.TryGetValue("callback", out string callbackcache);
            ViewBag.Callback= callbackcache;
            ViewBag.Time = timecache;

            ViewBag.Product = _memoryCache.Get<Product>("product:1");
            //ViewBag.Time = _memoryCache.Get<string>("time");
            return View();
        }
    }
}
