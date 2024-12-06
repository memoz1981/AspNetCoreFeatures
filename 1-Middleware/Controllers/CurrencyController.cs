using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace _1_Middleware.Controllers
{
    public class CurrencyController : Controller
    {
        public IActionResult Index()
        {
            var url = Url.Action(nameof(View), "Currency", new { code = "USD" });
            return Content($"The URL is {url}");
        }

        public new IActionResult View(string code)
        {
            return Ok(code); 
        }
    }
}
