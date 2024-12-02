using Microsoft.AspNetCore.Mvc;

namespace _2_Controllers.Controllers; 

[Route("[controller]")]
[ApiController]
public class Test2Controller : ControllerBase
{
    [HttpGet]
    public virtual IActionResult Get()
        => Ok("Test2 controller entered..."); 
}
