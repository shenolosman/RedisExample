using Microsoft.AspNetCore.Mvc;
using NuGet.Packaging;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers
{
    public class HashController : Controller
    {
        private readonly RedisService redisService;
        IDatabase db;
        public string hashList { get; set; } = "dictionary";
        public HashController(RedisService redisService)
        {
            this.redisService = redisService;
            db = redisService.GetDb(4);
        }
        public async Task<IActionResult> Index()
        {
            Dictionary<string,string> hashSet = new();
            if (await db.KeyExistsAsync(hashList))
            {
                var list=await db.HashGetAllAsync(hashList);
                list.ToList().ForEach(x => hashSet.Add(x.Name.ToString(),x.Value.ToString()));
            }
            return View(hashSet);
        }
        [HttpPost]
        public async Task<IActionResult> Add(string name, string value)
        {
            //await db.KeyExpireAsync(sortedSet, DateTime.Now.AddMinutes(5));

            await db.HashSetAsync(hashList, name, value);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(string name)
        {
            await db.HashDeleteAsync(hashList, name);
            return RedirectToAction("Index");
        }
    }
}
