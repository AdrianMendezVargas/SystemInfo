using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SystemInfo.Services;
using SystemInfo.Shared.Enums;
using SystemInfo.Shared.Requests;
using SystemInfo.Shared.Responses;

namespace SystemInfo.Api.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase {
        private readonly IAuthenticationService _authService;

        public AuthenticationController(IAuthenticationService authenticationService) {
            _authService = authenticationService;
        }

        [ProducesResponseType(200 , Type = typeof(OperationResponse<TokenResponse>))]
        [ProducesResponseType(400 , Type = typeof(EmptyOperationResponse))]
        [HttpPost]
        public IActionResult Post(TokenRequest tokenRequest) {
            var result =  _authService.GetToken(tokenRequest);
            if (result.OperationResult == ServiceResult.Success) {
                return Ok(result);
            } else {
                return BadRequest(new EmptyOperationResponse {
                    Message = result.Message ,
                    OperationResult = result.OperationResult ,
                });
            }
        }

    }
}
