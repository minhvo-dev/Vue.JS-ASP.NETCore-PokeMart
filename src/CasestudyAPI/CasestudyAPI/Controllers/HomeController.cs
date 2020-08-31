using Microsoft.AspNetCore.Mvc;
using CasestudyAPI.Controllers;

namespace CasestudyAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> Index()
        {
            return "server started";
        }
    }
}