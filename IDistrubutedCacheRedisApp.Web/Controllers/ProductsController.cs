using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace IDistrubutedCacheRedisApp.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IDistributedCache _distrubutedCache;

        public ProductsController(IDistributedCache distrubutedCache)
        {
            _distrubutedCache = distrubutedCache;
        }
        public async Task<IActionResult> Index()
        {
            DistributedCacheEntryOptions opt = new DistributedCacheEntryOptions();
            opt.AbsoluteExpiration = DateTime.Now.AddMinutes(1);

            await _distrubutedCache.SetStringAsync("name", "shenol", opt);
            return View();
        }

        public async Task<IActionResult> Show()
        {
            ViewBag.Data = await _distrubutedCache.GetStringAsync("name");
            return View();
        }

        public async Task<IActionResult> Remove()
        {
            await _distrubutedCache.RemoveAsync("name");
            return View();
        }
    }
}
