using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class EnterpriseController : ControllerBase {
        private readonly IEnterpriseService _enterpriseService;

        public EnterpriseController(IEnterpriseService enterpriseService) {
            _enterpriseService = enterpriseService;
        }

        [ProducesResponseType(200 , Type = typeof(OperationResponse<EnterpriseDetails>))]
        [ProducesResponseType(400 , Type = typeof(EmptyOperationResponse))]
        [HttpPost]
        public async Task<IActionResult> Post(CreateEnterpriseRequest enterpriseRequest) {
            var result = await _enterpriseService.CreateAsync(enterpriseRequest);
            if (result.OperationResult == ServiceResult.Success) {
                return Ok(new OperationResponse<EnterpriseDetails> {
                    Message = result.Message ,
                    OperationResult = result.OperationResult ,
                    Record = result.Record.ToEnterpriseDetails()
                });
            } else {
                return BadRequest(new EmptyOperationResponse {
                    Message = result.Message ,
                    OperationResult = result.OperationResult ,
                });
            }
        }

        [ProducesResponseType(200 , Type = typeof(OperationResponse<EnterpriseDetails>))]
        [ProducesResponseType(400 , Type = typeof(EmptyOperationResponse))]
        [HttpGet("{rnc}")]
        public async Task<IActionResult> Get(string rnc) {
            var result = await _enterpriseService.GetByRncAsync(rnc);
            if (result.OperationResult == ServiceResult.Success) {
                return Ok(new OperationResponse<EnterpriseDetails> {
                    Message = result.Message ,
                    OperationResult = result.OperationResult ,
                    Record = result.Record.ToEnterpriseDetails()
                });
            } else {
                return BadRequest(new EmptyOperationResponse {
                    Message = result.Message ,
                    OperationResult = result.OperationResult ,
                });
            }
        }

        [ProducesResponseType(200 , Type = typeof(OperationResponse<List<EnterpriseDetails>>))]
        [ProducesResponseType(400 , Type = typeof(EmptyOperationResponse))]
        [HttpGet]
        public async Task<IActionResult> GetEnterprises() {
            var result = await _enterpriseService.GetEnterprisesAsync();
            if (result.OperationResult == ServiceResult.Success) {
                return Ok(new OperationResponse<List<EnterpriseDetails>> {
                    Message = result.Message ,
                    OperationResult = result.OperationResult ,
                    Record = result.Record.ToListEnterpriseDetails()
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
