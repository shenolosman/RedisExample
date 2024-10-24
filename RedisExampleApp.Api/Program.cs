
using Microsoft.EntityFrameworkCore;
using RedisExampleApp.Api.Data;
using RedisExampleApp.Api.Data.Repositories;
using RedisExampleApp.Api.Services;
using RedisExampleApp.ApiCache;
using StackExchange.Redis;

namespace RedisExampleApp.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("expDatabase"));
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<IProductService, ProductService>();
            //to initiate cache decorator we get first usual repository than equal to cache repository
            builder.Services.AddScoped<IProductRepository>(sp =>
            {
                var appDbContext = sp.GetRequiredService<AppDbContext>();
                var productRepository = new ProductRepository(appDbContext);
                var redisService = sp.GetRequiredService<RedisService>();
                return new ProductRepositoryWithCacheDecorator(productRepository, redisService);
            });
            builder.Services.AddSingleton<RedisService>(opt =>
            {
                return new RedisService(builder.Configuration["CacheOptions:Url"]);
            });
            //builder.Services.AddSingleton<IDatabase>(opt =>
            //{
            //    var redisService = opt.GetRequiredService<RedisService>();
            //    return redisService.GetDb(0);
            //}); //set one of redis db and use in ctor
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Database.EnsureCreated();
            }
            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
