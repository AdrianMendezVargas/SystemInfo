using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SystemInfo.Models.Domain;

namespace SystemInfo.Repository {
    public interface ISystemSpecsRepository {
        public Task CreateAsync(SystemSpecs systemSpecs);
        public Task<SystemSpecs> FindByIdAsync(int systemSpecsId);
        public void Update(SystemSpecs systemSpecs);
        public void Remove(SystemSpecs systemSpecs);
        Task<List<SystemSpecs>> GetSystemSpecsByEnterpriseRncAsync(string rnc);
        Task<SystemSpecs> FindByMachineNameOnRncAsync(string machineName , string enterpriseRNC);
    }
}
