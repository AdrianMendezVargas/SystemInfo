using System.Threading.Tasks;
using SystemInfo.Models.Data;

namespace SystemInfo.Repository {
    public class EfUnitOfWork : IUnitOfWork {

        private readonly ApplicationDbContext _db;
        private ISystemSpecsRepository _systemSpecsRepository;
        private IEnterpriseRepository _enterpriseRepository;

        public EfUnitOfWork(ApplicationDbContext db) {
            _db = db;
        }

        public ISystemSpecsRepository SystemSpecsRepository {
            get {
                return _systemSpecsRepository ??= new SystemSpecsRepository(_db);
            }
        }

        public IEnterpriseRepository EnterpriseRepository {
            get {
                return _enterpriseRepository ??= new EnterpriseRepository(_db);
            }
        }

        public async Task<bool> CommitChangesAsync() {
            return (await _db.SaveChangesAsync()) > 0;
        }
    }
}
