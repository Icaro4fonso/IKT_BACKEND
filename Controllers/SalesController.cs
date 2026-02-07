using Microsoft.AspNetCore.Mvc;

namespace IKT_BACKEND.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalesController : ControllerBase
    {

        [HttpGet]
        public IActionResult ApiCheck()
        {
            return Ok("Hello World");
        }
    }
}
