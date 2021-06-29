using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemInfo.Models.Domain;
using SystemInfo.Shared.Requests;
using SystemInfo.Shared.Responses;

namespace SystemInfo.Services {
    public interface ISystemSpecsService {

        Task<OperationResponse<SystemSpecs>> CreateAsync(CreateSystemSpecsRequest saveSpecsRequest);
        Task<OperationResponse<SystemSpecs>> DeleteAsync(int taskId);
        Task<OperationResponse<SystemSpecs>> GetByIdAsync(int taskId);
        Task<OperationResponse<SystemSpecs>> UpdateAsync(CreateSystemSpecsRequest saveSpecsRequest);
        Task<OperationResponse<List<SystemSpecs>>> GetSystemSpecsByEnterpriseRncAsync(string rnc);

    }

}
