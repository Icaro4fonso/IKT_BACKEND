using IKT_BACKEND.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace IKT_BACKEND.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalesController : ControllerBase
    {

        private readonly ISalesService salesService;

        public SalesController(ISalesService salesService)
        {
            this.salesService = salesService;
        }

        [HttpPost("upload-records")]
        [Consumes("multipart/form-data")]
        [RequestSizeLimit(10 * 1024 * 1024)] // 10 MB
        public async Task<IActionResult> SaveRecords([FromForm] IFormFile file)
        {
            var response = await salesService.SaveRecords(file);

            return Ok();
        }

        [HttpGet("most-profit-month")]
        public async Task<IActionResult> MostProfitMonths()
        {
            var response = await salesService.MostProfitMonths();
            if (response.Success)
            {
                return Ok(response.GetValue());
            }
            return BadRequest();
        }


        public IActionResult ApiCheck()
        {
            var sales = salesService.GetSuccess();

            if (sales.Success)
            {
                return Ok(sales.GetValue());
            }

            return Ok("Hello World");
        }
    }
}
