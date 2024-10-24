using Microsoft.AspNetCore.Mvc;
using NuGet.Packaging;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers
{
    public class SortedSetController : Controller
    {
        private readonly RedisService redisService;
        IDatabase db;
        string sortedSet = "sortedSet";
        public SortedSetController(RedisService redisService)
        {
            this.redisService = redisService;
            db = redisService.GetDb(3);
        }
        public async Task<IActionResult> Index()
        {
            HashSet<string> hashSet = new HashSet<string>();
            if (await db.KeyExistsAsync(sortedSet))
            {
                db.SortedSetScan(sortedSet).ToList().ForEach(x =>hashSet.Add(x.ToString()));

                //var list = await db.SortedSetRangeByRankAsync(sortedSet, order: Order.Ascending);
                //list.ToList().ForEach(x => hashSet.Add(x.ToString()));
            }
            return View(hashSet);
        }
        [HttpPost]
        public async Task<IActionResult> Add(string name, int score)
        {
            //await db.KeyExpireAsync(sortedSet, DateTime.Now.AddMinutes(5));

            await db.SortedSetAddAsync(sortedSet, name, score);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(string name)
        {
            await db.SortedSetRemoveAsync(sortedSet, name);
            return RedirectToAction("Index");
        }
    }
}
