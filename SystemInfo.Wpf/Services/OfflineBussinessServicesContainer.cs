using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using SystemInfo.Repository;
using SystemInfo.Services;
using SystemInfo.Wpf.Data;

namespace SystemInfo.Wpf.Services {
    public static class OfflineBussinessServicesContainer {

        private static ISystemSpecsService _systemSpecsService;
        private static IEnterpriseService _enterpriseService;
        private static PreferencesService _preferencesService;

        public static ISystemSpecsService SystemSpecsService {
            get {
                return _systemSpecsService ??= new SystemSpecsService(GetOfflineUnit());
            }
        }

        public static IEnterpriseService EnterpriseService {
            get {
                return _enterpriseService ??= new EnterpriseService(GetOfflineUnit());
            }
        }

        public static PreferencesService PreferencesService {
            get {
                return _preferencesService ??= new PreferencesService(GetOfflineDbContext());
            }
        }


        private static IUnitOfWork GetOfflineUnit() {
            return new EfUnitOfWork(GetOfflineDbContext());
        }

        public static OfflineApplicationDbContext GetOfflineDbContext() {
            return new OfflineApplicationDbContext();
        }

    }
}
