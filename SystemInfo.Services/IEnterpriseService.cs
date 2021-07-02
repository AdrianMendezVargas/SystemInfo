using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemInfo.Models.Domain;
using SystemInfo.Models.Mappers;
using SystemInfo.Repository;
using SystemInfo.Shared.Extensions;
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

        public async Task<OperationResponse<List<CreateEnterpriseRequest>>> CreateRangeAsync(List<CreateEnterpriseRequest> enterpriseRequestList) {
            var invalidEnterprisesRequest = new List<CreateEnterpriseRequest>();

            enterpriseRequestList.ForEach(async (enterpriseRequest) => {
                if (!enterpriseRequest.IsValid()) {
                    invalidEnterprisesRequest.Add(enterpriseRequest);
                } else {
                    var enterprise = enterpriseRequest.ToEnterprise();
                    enterprise.ModifiedOn = null;
                    enterprise.CreatedOn = DateTime.Now;

                    try {
                        await _unit.EnterpriseRepository.CreateAsync(enterprise);

                    } catch (Exception) {
                        invalidEnterprisesRequest.Add(enterpriseRequest);
                    }
                }
            });

            bool done = await _unit.CommitChangesAsync();
            string successMessage = invalidEnterprisesRequest.Any() 
                ? "Algunas empresas no pudieron guardarse" 
                : "Las empresas se guardaron exitosamente";
            return done ? Success(successMessage , invalidEnterprisesRequest)
                        : Error("No pudieron guardar las empresas" , enterpriseRequestList);
        }

        public Task<OperationResponse<Enterprise>> DeleteAsync(string enterpriseRnc) {
            throw new NotImplementedException();
        }

        public async Task<OperationResponse<Enterprise>> GetByRncAsync(string enterpriseRnc) {
            if (!enterpriseRnc.IsRncValid()) {
                return Error("El RNC es invalido" , new Enterprise());
            }

            var enterprise = await _unit.EnterpriseRepository.FindByRncAsync(enterpriseRnc);
            if (enterprise == null) {
                return Error("No existe una empresa con este RNC" , new Enterprise());
            }

            return Success("Empresa encontrada" , enterprise);
        }

        public async Task<OperationResponse<List<Enterprise>>> GetEnterprisesAsync() {
            var enterprises = await _unit.EnterpriseRepository.GetEnterprisesAsync();
            return Success("" , enterprises);
        }

        public Task<OperationResponse<Enterprise>> UpdateAsync(CreateEnterpriseRequest enterpriseRequest) {
            throw new NotImplementedException();
        }
    }

}
