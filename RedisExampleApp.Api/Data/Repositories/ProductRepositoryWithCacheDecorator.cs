using RedisExampleApp.Api.Models;
using RedisExampleApp.ApiCache;
using StackExchange.Redis;
using System.Text.Json;

namespace RedisExampleApp.Api.Data.Repositories
{
    public class ProductRepositoryWithCacheDecorator : IProductRepository
    {
        private readonly IProductRepository _productRepository;
        private readonly RedisService _redisService;
        //this can be written in service as Decorator Design Pattern
        private const string productKey = "ProductCache";
        private readonly IDatabase _cacheRepository;
        public ProductRepositoryWithCacheDecorator(IProductRepository productRepository, RedisService redisService)
        {
            _productRepository = productRepository;
            _redisService = redisService;
            _cacheRepository = _redisService.GetDb(5);
        }
        async Task<Product> IProductRepository.CreateAsync(Product product)
        {
            var newProduct = await _productRepository.CreateAsync(product);

            if (await _cacheRepository.KeyExistsAsync(productKey))
                await _cacheRepository.HashSetAsync(productKey, product.Id, JsonSerializer.Serialize<Product>(product));

            return newProduct;
        }

        async Task<List<Product>> IProductRepository.GetAllAsync()
        {

            var checkProduct = await _cacheRepository.KeyExistsAsync(productKey);
            if (!checkProduct)
                return await LoadToCacheFromDbAsync();

            var products = new List<Product>();

            var cachedData = await _cacheRepository.HashGetAllAsync(productKey);
            foreach (var item in cachedData)
            {
                var product = JsonSerializer.Deserialize<Product>(item.Value.ToString());
                products.Add(product);
            }
            return products;
        }

        async Task<Product> IProductRepository.GetByIdAsync(int id)
        {
            if (await _cacheRepository.KeyExistsAsync(productKey))
            {
                var product = await _cacheRepository.HashGetAsync(productKey, id);
                return product.HasValue ? JsonSerializer.Deserialize<Product>(product) : null;
            }
            var products = await LoadToCacheFromDbAsync();

            return products.FirstOrDefault(x => x.Id == id);
        }
        private async Task<List<Product>> LoadToCacheFromDbAsync()
        {
            var products = await _productRepository.GetAllAsync();

            products.ForEach(product =>
            {
                _cacheRepository.HashSetAsync(productKey, product.Id, JsonSerializer.Serialize(product));
            });
            return products;
        }
    }
}
