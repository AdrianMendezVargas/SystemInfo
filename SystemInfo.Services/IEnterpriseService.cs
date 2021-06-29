using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemInfo.Models.Domain;
using SystemInfo.Models.Mappers;
using SystemInfo.Repository;
using SystemInfo.Shared.Requests;
using SystemInfo.Shared.Responses;

namespace SystemInfo.Services {
    public interface IEnterpriseService {
        Task<OperationResponse<Enterprise>> CreateAsync(CreateEnterpriseRequest enterpriseRequest);
        Task<OperationResponse<Enterprise>> DeleteAsync(int enterpriseId);
        Task<OperationResponse<Enterprise>> GetByIdAsync(int enterpriseId);
        Task<OperationResponse<Enterprise>> UpdateAsync(CreateEnterpriseRequest enterpriseRequest);
        Task<OperationResponse<List<Enterprise>>> GetEnterprisesAsync();
    }

    public class EnterpriseService : BaseService, IEnterpriseService {
        private readonly IUnitOfWork _unit;

        public EnterpriseService(IUnitOfWork unit) {
            _unit = unit;
        }

        public async Task<OperationResponse<Enterprise>> CreateAsync(CreateEnterpriseRequest enterpriseRequest) {

            if (!enterpriseRequest.IsValid()) {
                return Error("Su petición es invalida" , new Enterprise { });
            }

            var enterprise = enterpriseRequest.ToEnterprise();
            #region setting default values to enterprise
            enterprise.ModifiedOn = null;
            enterprise.CreatedOn = DateTime.Now;
            #endregion

            await _unit.EnterpriseRepository.CreateAsync(enterprise);
            var done = await _unit.CommitChangesAsync();
            return done ? Success("La empresa se guardo exitosamente" , enterprise)
                        : Error("No se pudo guardar la empresa" , new Enterprise { });
        }

        public Task<OperationResponse<Enterprise>> DeleteAsync(int enterpriseId) {
            throw new NotImplementedException();
        }

        public Task<OperationResponse<Enterprise>> GetByIdAsync(int enterpriseId) {
            throw new NotImplementedException();
        }

        public Task<OperationResponse<List<Enterprise>>> GetEnterprisesAsync() {
            throw new NotImplementedException();
        }

        public Task<OperationResponse<Enterprise>> UpdateAsync(CreateEnterpriseRequest enterpriseRequest) {
            throw new NotImplementedException();
        }
    }

}
