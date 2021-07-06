using Microsoft.EntityFrameworkCore.Query.Internal;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SystemInfo.Models.Domain;
using SystemInfo.Shared.Models;
using SystemInfo.Shared.Requests;
using SystemInfo.Shared.Responses;

namespace SystemInfo.Services {
    public interface IEnterpriseService {
        Task<OperationResponse<Enterprise>> CreateAsync(CreateEnterpriseRequest enterpriseRequest);
        Task<OperationResponse<Enterprise>> DeleteAsync(string enterpriseRnc);
        Task<OperationResponse<Enterprise>> GetByRncAsync(string enterpriseRnc);
        Task<OperationResponse<Enterprise>> UpdateAsync(CreateEnterpriseRequest enterpriseRequest);
        Task<OperationResponse<List<Enterprise>>> GetEnterprisesAsync();
    }

}
