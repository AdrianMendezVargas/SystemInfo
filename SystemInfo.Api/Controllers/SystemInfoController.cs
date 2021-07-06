using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SystemInfo.Models.Mappers;
using SystemInfo.Services;
using SystemInfo.Shared.Enums;
using SystemInfo.Shared.Models;
using SystemInfo.Shared.Requests;
using SystemInfo.Shared.Responses;

namespace SystemInfo.Api.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class SystemInfoController : ControllerBase {
        private readonly ISystemSpecsService _specsService;

        public SystemInfoController(ISystemSpecsService specsService) {
            _specsService = specsService;
        }

        [ProducesResponseType(200 , Type = typeof(OperationResponse<SystemSpecsDetails>))]
        [ProducesResponseType(400 , Type = typeof(EmptyOperationResponse))]
        [HttpPost]
        public async Task<IActionResult> Post(CreateSystemSpecsRequest saveRequest) {
            var result = await _specsService.CreateAsync(saveRequest);
            if (result.OperationResult == ServiceResult.Success) {
                return Ok(new OperationResponse<SystemSpecsDetails> {
                    Message = result.Message ,
                    OperationResult = result.OperationResult ,
                    Record = result.Record.ToSystemSpecsDetails()
                });
            } else {
                return BadRequest(new EmptyOperationResponse {
                    Message = result.Message ,
                    OperationResult = result.OperationResult ,
                });
            }
        }

    }
}
