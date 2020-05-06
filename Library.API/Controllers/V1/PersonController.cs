using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.API.Controllers.V1
{
    [Route("api/[Controller]")]
    [ApiVersion("1.0")]
    //[ApiVersion("2.0")]
    public class PersonController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> Get() => "Result from V1";

        //[HttpGet, MapToApiVersion("2.0")]
        //public ActionResult<string> GetV2() => "Result from V1";
    }

}
