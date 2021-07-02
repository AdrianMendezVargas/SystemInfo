using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SystemInfo.Models.Mappers;
using SystemInfo.Services;
using SystemInfo.Shared.Models;
using SystemInfo.Shared.Requests;
using SystemInfo.Shared.Responses;

namespace SystemInfo.Api.Controllers {
    [Route("api/[controller]")]
    [ApiController]
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
            if (result.IsSuccess) {
                return Ok(new OperationResponse<EnterpriseDetails> {
                    Message = result.Message ,
                    IsSuccess = result.IsSuccess ,
                    Record = result.Record.ToEnterpriseDetails()
                });
            } else {
                return BadRequest(new EmptyOperationResponse {
                    Message = result.Message ,
                    IsSuccess = result.IsSuccess ,
                });
            }
        }

        [ProducesResponseType(200 , Type = typeof(OperationResponse<EnterpriseDetails>))]
        [ProducesResponseType(400 , Type = typeof(EmptyOperationResponse))]
        [HttpGet("{rnc}")]
        public async Task<IActionResult> Get(string rnc) {
            var result = await _enterpriseService.GetByRncAsync(rnc);
            if (result.IsSuccess) {
                return Ok(new OperationResponse<EnterpriseDetails> {
                    Message = result.Message ,
                    IsSuccess = result.IsSuccess ,
                    Record = result.Record.ToEnterpriseDetails()
                });
            } else {
                return BadRequest(new EmptyOperationResponse {
                    Message = result.Message ,
                    IsSuccess = result.IsSuccess ,
                });
            }
        }

    }

}
