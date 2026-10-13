using Microsoft.AspNetCore.Mvc;

namespace TrafficFineManagement.Web.Controllers
{
    [Route("car")]
    public class CarController : Controller
    {
        [HttpGet("create")]
        public IActionResult Create()
        {
            return View();
        }
    }
}
