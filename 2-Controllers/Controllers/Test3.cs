using Microsoft.AspNetCore.Mvc;

namespace _2_Controllers.Controllers; 

public class Test3 : Test2Controller
{
    [HttpGet]
    public override IActionResult Get() => Ok("Test3 controller entered..."); 
}
