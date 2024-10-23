using IDistrubutedCacheRedisApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

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

            //await _distrubutedCache.SetStringAsync("name", "shenol", opt);

            Product product = new Product() { Id = 1, Name = "pen", Price = 200 };
            string jsonProduct = JsonSerializer.Serialize(product);

            byte[] byteproduct = Encoding.UTF8.GetBytes(jsonProduct);


            await _distrubutedCache.SetStringAsync("product:1", jsonProduct, opt);
            await _distrubutedCache.SetAsync("product:2", byteproduct, opt);

            return View();
        }

        public async Task<IActionResult> Show()
        {
            byte[] byteProduct = await _distrubutedCache.GetAsync("product:2");
            var productData = await _distrubutedCache.GetStringAsync("product:1");
            Product p = JsonSerializer.Deserialize<Product>(productData);
            string jsonProduct = Encoding.UTF8.GetString(byteProduct);

            Product p2 = JsonSerializer.Deserialize<Product>(jsonProduct);

            ViewBag.Data = p;
            ViewBag.Data2 = p2;
            return View();
        }

        public async Task<IActionResult> Remove()
        {
            await _distrubutedCache.RemoveAsync("product:1");
            return View();
        }

        public async Task<IActionResult> ImageCache()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/devops.png");

            byte[] imageByte= System.IO.File.ReadAllBytes(path);

            await _distrubutedCache.SetAsync("image", imageByte);

            return View();
        }

        public async Task<IActionResult> ImageUrl()
        {
            byte[] imageByte = await _distrubutedCache.GetAsync("image");


            return File(imageByte,"image/png");
        }
    }
}
