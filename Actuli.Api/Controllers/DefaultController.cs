using Microsoft.AspNetCore.Mvc;

namespace Actuli.Api.Controllers
{
    [Route("api/")]
    public class DefaultController : ControllerBase
    {
        [HttpGet]
        public IActionResult Index()
        {
            return Ok("Welcome to the Actuli Backend API!");
        }
        
        [HttpGet("HelloWorld")]
        public IActionResult HelloWorld()
        {
            return Ok("Hello World!");
        }
    }
}