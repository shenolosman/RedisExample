
using Microsoft.AspNetCore.Mvc;
using RedisExampleApp.Api.Data.Repositories;
using RedisExampleApp.Api.Models;
using RedisExampleApp.Api.Services;

namespace RedisExampleApp.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        
        public ProductsController(IProductService productService)
        {
            _productService = productService;

        }

        // GET: Products
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var allProducts = await _productService.GetAllAsyncT();
            return Ok(allProducts);
        }

        // GET: Products/Details/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var findedProduct = await _productService.GetByIdAsyncT(id);
            return Ok(findedProduct);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            var createdProduct = await _productService.CreateAsyncT(product);
            return Created(string.Empty, createdProduct);
        }

    }
}
