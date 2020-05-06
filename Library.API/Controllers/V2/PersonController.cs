using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers.V2
{

    [Route("api/[Controller]")]
    [ApiVersion("2.0")]
    //[Route("api/v{version:apiVersion}/[controller]")]
    public class PersonController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> Get() => "Result from V2";
    }

}
