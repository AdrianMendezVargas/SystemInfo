using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SystemInfo.Models.Domain;
using SystemInfo.Models.Mappers;
using SystemInfo.Repository;
using SystemInfo.Shared.Enums;
using SystemInfo.Shared.Requests;
using SystemInfo.Shared.Responses;

namespace SystemInfo.Services {
    public class SystemSpecsService : BaseService, ISystemSpecsService {
        private readonly IUnitOfWork _unit;

        public SystemSpecsService(IUnitOfWork unit) {
            _unit = unit;
        }

        public async Task<OperationResponse<SystemSpecs>> CreateAsync(CreateSystemSpecsRequest saveSpecsRequest) {

            bool enterpriseExist = await _unit.EnterpriseRepository.FindByRncAsync(saveSpecsRequest.EnterpriseRNC) != null;
            if (!enterpriseExist) {
                return Error("No existe la empresa con el RNC indicado" , new SystemSpecs { }, ServiceResult.InvalidData);
            }

            bool machineNameAlreadyRegistered = await _unit.SystemSpecsRepository.FindByMachineNameOnRncAsync(saveSpecsRequest.MachineName, saveSpecsRequest.EnterpriseRNC) != null;
            if (machineNameAlreadyRegistered) {
                return Error("Ya existe una maquina con este nombre en la empresa" , new SystemSpecs { }, ServiceResult.AlreadyExist);
            }

            var systemSpecs = saveSpecsRequest.ToSystemSpecs();

            #region Setting default values to systemSpecs
            systemSpecs.Id = 0;
            systemSpecs.CreatedOn = DateTime.Now;
            systemSpecs.ModifiedOn = null;
            #endregion

            await _unit.SystemSpecsRepository.CreateAsync(systemSpecs);
            var done = await _unit.CommitChangesAsync();
            return done ? Success("La especificaciones se guardaron exitosamente" , systemSpecs)
                        : Error("No se pudo guardar la especificaciones" , new SystemSpecs { }, ServiceResult.Unknown);
        }

        public Task<OperationResponse<SystemSpecs>> DeleteAsync(int taskId) {
            throw new NotImplementedException();
        }

        public Task<OperationResponse<SystemSpecs>> GetByIdAsync(int taskId) {
            throw new NotImplementedException();
        }

        public Task<OperationResponse<List<SystemSpecs>>> GetSystemSpecsByEnterpriseRncAsync(string rnc) {
            throw new NotImplementedException();
        }

        public Task<OperationResponse<SystemSpecs>> UpdateAsync(CreateSystemSpecsRequest saveSpecsRequest) {
            throw new NotImplementedException();
        }
    }

}
