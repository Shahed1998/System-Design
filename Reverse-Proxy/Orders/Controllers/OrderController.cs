using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Orders.Controllers
{

    public class OrderController : BaseController
    {
        [HttpGet("GetOrders")]
        public IActionResult Get()
        {
            return Ok(new { message = "Hello World From Orders" });
        }
    }
}
