using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SystemInfo.Models.Data;
using SystemInfo.Models.Domain;

namespace SystemInfo.Repository {


    public class SystemSpecsRepository : ISystemSpecsRepository {
        private readonly ApplicationDbContext _db;

        public SystemSpecsRepository(ApplicationDbContext db) {
            _db = db;
        }

        public async Task CreateAsync(SystemSpecs systemSpecs) {
            await _db.SystemSpecs.AddAsync(systemSpecs);
        }

        public async Task<SystemSpecs> FindByIdAsync(int systemSpecsId) {
            return await _db.SystemSpecs.SingleOrDefaultAsync(s => s.Id == systemSpecsId);
        }

        public void Remove(SystemSpecs systemSpecs) {
            _db.SystemSpecs.Remove(systemSpecs);
        }

        public void Update(SystemSpecs systemSpecs) {
            _db.SystemSpecs.Update(systemSpecs);
        }

        public async Task<List<SystemSpecs>> GetSystemSpecsByEnterpriseRncAsync(string rnc) {
            return await _db.SystemSpecs.Where(s => s.EnterpriseRNC == rnc)
                .Include(s => s.HardDisks)
                .ToListAsync();
        }

        public async Task<SystemSpecs> FindByMachineNameOnRncAsync(string machineName , string enterpriseRNC) {
            return await _db.SystemSpecs.SingleOrDefaultAsync(s => s.MachineName == machineName && s.EnterpriseRNC == enterpriseRNC);
        }
    }
}
