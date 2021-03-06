using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SystemInfo.Api.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class ConnectionController : ControllerBase {

        [ProducesResponseType(200 , Type = typeof(bool))]
        [HttpGet]
        public IActionResult Get() {
            return Ok(true);
        }

    }
}
