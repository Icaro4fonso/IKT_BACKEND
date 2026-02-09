using IKT_BACKEND.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace IKT_BACKEND.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {

        private readonly IProductService productService;

        public ProductsController(IProductService productService)
        {
            this.productService = productService;
        }

        [HttpGet("most-ordered")]
        public async Task<IActionResult> MostOrdereds()
        {
            var response = await productService.MostOrdereds();
            if (response.Success)
            {

                return Ok(response.GetValue());
            }

            return BadRequest();
        }
    }
}
