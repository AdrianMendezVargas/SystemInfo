﻿using System;
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


        private static IUnitOfWork GetOfflineUnit() {
            return new EfUnitOfWork(new OfflineApplicationDbContext());
        }

    }
}