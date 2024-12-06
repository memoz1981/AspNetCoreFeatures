using Microsoft.AspNetCore.Mvc;

namespace _2_Controllers.Controllers; 

public class Test3 : Test2Controller
{
    // if override (and virual in parent) not used - AmbigousMatchException is thrown
    // attribute routing should return single match (rather than first match in conventional routing)
    // method name can be different as well - in that case route name would probably need to be specified different from parent
    [HttpGet]
    public override IActionResult Get() => Ok("Test3 controller entered..."); 
}
