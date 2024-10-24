using RedisExampleApp.Api.Models;

namespace RedisExampleApp.Api.Services
{
    public interface IProductService
    {
        Task<List<Product>> GetAllAsyncT();
        Task<Product> GetByIdAsyncT(int id);
        Task<Product> CreateAsyncT(Product product);
    }
}
