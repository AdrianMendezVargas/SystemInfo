using System.Collections.Generic;
using System.Threading.Tasks;
using SystemInfo.Models.Domain;

namespace SystemInfo.Repository {
    public interface IHardDiskRepository {
        public Task CreateAsync(HardDisk hardDisk);
        public Task<HardDisk> FindByIdAsync(int hardDiskId);
        public void Update(HardDisk hardDisk);
        public void Remove(HardDisk hardDisk);
    }
}
