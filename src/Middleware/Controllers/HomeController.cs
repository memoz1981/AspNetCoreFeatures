using Microsoft.AspNetCore.Mvc;

namespace Middleware.Controllers; 

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return Ok("Received request to Home/Index method");
    }
}
