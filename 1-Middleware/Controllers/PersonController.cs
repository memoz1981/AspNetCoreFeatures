using Microsoft.AspNetCore.Mvc;

namespace _1_Middleware.Controllers
{
    public class PersonController : Controller
    {
        public IActionResult Show(string name)
        {
            return Ok("show..."); 
        }

        public IActionResult Data()
        {
            return Ok("data...");
        }
    }
}
