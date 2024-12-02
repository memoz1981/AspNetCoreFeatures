using Microsoft.AspNetCore.Mvc;

namespace _2_Controllers.Controllers; 

public class Test1Controller : Controller
{
    public IActionResult Index()
        => Ok("Test1 controller entered");
}
