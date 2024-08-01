using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MetoFirstExample_v4_WepAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class testController : ControllerBase
    {
        public ActionResult<string> Get()
        {
            return Content("ok");
        }
       
    }
}
