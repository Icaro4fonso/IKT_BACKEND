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

        [HttpPost("upload-records")]
        [Consumes("multipart/form-data")]
        [RequestSizeLimit(10 * 1024 * 1024)] // 10 MB
        public async Task<IActionResult> SaveRecords([FromForm] IFormFile file)
        {
            var response = await SalesService.SaveRecords(file);

            return Ok();
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
