using System.Collections.Generic;
using System.Threading.Tasks;
using SystemInfo.Models.Domain;

namespace SystemInfo.Repository {
    public interface IWindowsAccountRepository {
        public Task CreateAsync(WindowsAccount windowsAccount);
        public Task<WindowsAccount> FindByIdAsync(int windowsAccountId);
        public void Update(WindowsAccount windowsAccount);
        public void Remove(WindowsAccount windowsAccount);
    }
}
