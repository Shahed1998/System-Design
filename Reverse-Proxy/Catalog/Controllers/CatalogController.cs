using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Controllers
{

    public class CatalogController : BaseController
    {
        [HttpGet("GetCatalog")]
        public IActionResult Get()
        {
            return Ok(new { message = "Hello World From Catalog" });
        }
    }
}
