using RedisExchangeAPI.Web.Services;

namespace RedisExchangeAPI.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddSingleton<RedisService>();

            var app = builder.Build();
            // Configure the HTTP request pipeline.

            using (var serviceScope = app.Services.CreateScope())
            {
                var redisService = serviceScope.ServiceProvider.GetRequiredService<RedisService>();
                redisService.Connect();
            }

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
           
            app.UseRouting();

            app.UseAuthorization();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}