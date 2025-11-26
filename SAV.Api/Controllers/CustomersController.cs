using Microsoft.AspNetCore.Mvc;
using SAV.Application.Dtos; // <--- Usamos Application

namespace SAV.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            var customers = new List<CustomerDto>
            {
                new CustomerDto { Id = 501, Name = "Laura Mendez", Email = "laura@example.com", Country = "Dominican Republic" },
                new CustomerDto { Id = 502, Name = "Carlos Santana", Email = "carlos@example.com", Country = "Mexico" },
                new CustomerDto { Id = 503, Name = "Ana Lima", Email = "ana@example.com", Country = "Peru" }
            };

            return Ok(customers);
        }
    }
}