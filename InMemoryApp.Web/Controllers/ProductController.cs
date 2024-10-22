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
            _memoryCache.Set<string>("time", DateTime.Now.ToString());
            return View();
        }

        public IActionResult ShowTime()
        {
            ViewBag.Time = _memoryCache.Get<string>("time");
            return View();
        }
    }
}
