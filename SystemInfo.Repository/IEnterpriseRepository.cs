using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemInfo.Models.Domain;

namespace SystemInfo.Repository {
    public interface IEnterpriseRepository {
        public Task CreateAsync(Enterprise enterprise);
        public Task<Enterprise> FindByRncAsync(string rnc);
        public void Update(Enterprise enterprise);
        public void Remove(Enterprise enterprise);
        Task<List<Enterprise>> GetEnterprises();
    }
}
