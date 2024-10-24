using RedisExampleApp.Api.Data.Repositories;
using RedisExampleApp.Api.Models;

namespace RedisExampleApp.Api.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        Task<Product> IProductService.CreateAsyncT(Product product)
        {
            //if mapper uses use it here with DTOs
            return _productRepository.CreateAsync(product);
        }

        Task<List<Product>> IProductService.GetAllAsyncT()
        {
            return _productRepository.GetAllAsync();
        }

        Task<Product> IProductService.GetByIdAsyncT(int id)
        {
           return _productRepository.GetByIdAsync(id);
        }
    }
}
