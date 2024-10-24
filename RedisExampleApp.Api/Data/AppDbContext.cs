using Microsoft.EntityFrameworkCore;
using RedisExampleApp.Api.Models;

namespace RedisExampleApp.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Product> Products { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasData(
                new Product() { Id = 1, Name = "pen", Price = 100, Description = "This is a pen!" },
                new Product() { Id = 2, Name = "pencil", Price = 200, Description = "This is a pencil!" },
                new Product() { Id = 3, Name = "marker", Price = 300 });
            base.OnModelCreating(modelBuilder);
        }
    }
}
