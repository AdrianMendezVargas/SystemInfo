using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemInfo.Repository {
    public interface IUnitOfWork {
        public ISystemSpecsRepository SystemSpecsRepository { get;}
        public IEnterpriseRepository EnterpriseRepository { get; }

        public Task<bool> CommitChangesAsync();

    }
}
