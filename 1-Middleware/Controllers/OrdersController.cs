using Microsoft.AspNetCore.Mvc;

namespace _1_Middleware.Controllers
{
    public class OrdersController : Controller
    {
        public IActionResult Get(int id)
        {
            return Ok($"Received id is {id}"); 
        }
    }
}
