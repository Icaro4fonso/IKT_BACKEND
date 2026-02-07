using IKT_BACKEND.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace IKT_BACKEND.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalesController : ControllerBase
    {

        private readonly ISalesService SalesService;

        public SalesController(ISalesService salesService)
        {
            SalesService = salesService;
        }

        [HttpGet]
        public IActionResult ApiCheck()
        {
            var sales = SalesService.GetSuccess();

            if (sales.Success)
            {
                return Ok(sales.GetValue());
            }

            return Ok("Hello World");
        }


    }
}
