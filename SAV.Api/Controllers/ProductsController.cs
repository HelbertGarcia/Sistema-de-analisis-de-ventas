using Microsoft.AspNetCore.Mvc;
using SAV.Application.Dtos; // <--- Ahora usamos el namespace de Application

namespace SAV.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            var products = new List<ProductDto>
            {
                new ProductDto { Id = 101, Title = "Laptop Gamer Pro", Price = 1500.00m, Category = "Electronics" },
                new ProductDto { Id = 102, Title = "Mouse Inalámbrico", Price = 25.50m, Category = "Accessories" },
                new ProductDto { Id = 103, Title = "Monitor 4K", Price = 300.99m, Category = "Electronics" },
                new ProductDto { Id = 104, Title = "Teclado Mecánico", Price = 89.99m, Category = "Accessories" }
            };

            return Ok(products);
        }
    }
}