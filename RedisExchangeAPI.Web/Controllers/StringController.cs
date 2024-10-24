using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers
{
    public class StringController : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabase db;
        public StringController(RedisService redisService)
        {
            _redisService = redisService;
            db = _redisService.GetDb(0);
        }
        public IActionResult Index()
        {
            db.StringSet("name", "Shenol O");
            db.StringSet("guess", 100);

            return View();
        }
        public IActionResult Show()
        {
            var v = db.StringGetRange("name", 0, 5);
            //var v = db.StringGet("name");

            if (v.HasValue)
            {
                ViewBag.v = v;
            }

            db.StringIncrement("guess", 10);
            db.StringDecrement("guess", 9);

            var v2 = db.StringGet("guess");


            if (v2.HasValue)
            {
                ViewBag.v2 = v2;
            }

            return View();
        }

    }
}
