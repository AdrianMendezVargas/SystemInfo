using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using SystemInfo.Models.Data;
using SystemInfo.Models.Domain;

namespace SystemInfo.Repository {
    public class EnterpriseRepository : IEnterpriseRepository {
        private readonly ApplicationDbContext _db;

        public EnterpriseRepository(ApplicationDbContext db) {
            _db = db;
        }

        public async Task CreateAsync(Enterprise enterprise) {
            await _db.Enterprises.AddAsync(enterprise);
        }

        public async Task<Enterprise> FindByRncAsync(string rnc) {
            return await _db.Enterprises.SingleOrDefaultAsync(e => e.RNC == rnc);
        }

        public void Remove(Enterprise enterprise) {
            _db.Enterprises.Remove(enterprise);
        }

        public void Update(Enterprise enterprise) {
            _db.Enterprises.Update(enterprise);
        }

        public async Task<List<Enterprise>> GetEnterprises() {
            return await _db.Enterprises.ToListAsync();
        }
    }
}
