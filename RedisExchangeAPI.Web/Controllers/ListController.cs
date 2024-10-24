using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers
{
    public class ListController : Controller
    {
        private readonly RedisService redisService;
        IDatabase db;
        string listKey = "listKey";
        public ListController(RedisService redisService)
        {
            this.redisService = redisService;
            db = redisService.GetDb(1);
        }
        public async Task<IActionResult> Index()
        {
            List<string> nameList = new List<string>();
            if (await db.KeyExistsAsync(listKey))
            {
                var list = await db.ListRangeAsync(listKey);
                list.ToList().ForEach(x => nameList.Add(x.ToString()));
            }
            return View(nameList);
        }
        [HttpPost]
        public async Task<IActionResult> Add(string name)
        {
            await db.ListRightPushAsync(listKey, name);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(string name)
        {
            await db.ListRemoveAsync(listKey, name);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteFirstItem()
        {
            await db.ListLeftPopAsync(listKey);
            return RedirectToAction("Index");
        }
    }
}
