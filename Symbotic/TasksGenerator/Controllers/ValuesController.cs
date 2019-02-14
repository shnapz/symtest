using Microsoft.AspNetCore.Mvc;

namespace TasksGenerator.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        [HttpGet]
        public string Get(int id)
        {
            return "value";
        }
    }
}