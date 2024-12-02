using Microsoft.AspNetCore.Mvc;

namespace _2_Controllers.Controllers
{
    public class Test5NotValidControler
    {
        [HttpGet]
        public IActionResult Get() => new OkObjectResult("Test5 controller shouldn't enter"); 
    }
}
