using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers
{
    public class SetController : Controller
    {
        private readonly RedisService redisService;
        IDatabase db;
        string listSet = "listSet";
        public SetController(RedisService redisService)
        {
            this.redisService = redisService;
            db = redisService.GetDb(2);
        }
        public async Task<IActionResult> Index()
        {
            HashSet<string> hashSet = new HashSet<string>();
            if (await db.KeyExistsAsync(listSet))
            {
                var myList = await db.SetMembersAsync(listSet);
                myList.ToList().ForEach(x => hashSet.Add(x.ToString()));
            }
            return View(hashSet);
        }
        [HttpPost]
        public async Task<IActionResult> Add(string name)
        {
            //if (!await db.KeyExistsAsync(listSet)) //to give sliding expression
            await db.KeyExpireAsync(listSet, DateTime.Now.AddMinutes(5));

            await db.SetAddAsync(listSet, name);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(string name)
        {
            await db.SetRemoveAsync(listSet, name);
            return RedirectToAction("Index");
        }
    }
}
