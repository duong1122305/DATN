using Microsoft.AspNetCore.Mvc;

namespace DATN.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        [HttpGet("lomma")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
