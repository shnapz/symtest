using Microsoft.AspNetCore.Mvc;
using Share.Models.Task;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    public class TestController : Controller
    {
        [HttpPost]
        public IActionResult Test([FromBody] MessageExternalApi messageExternalApi)
        {
            return Ok("Test Ok");
        }
    }
}