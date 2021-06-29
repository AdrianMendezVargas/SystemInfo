using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemInfo.Models.Domain;
using SystemInfo.Shared.Models;

namespace SystemInfo.Models.Mappers {
    public static class WindowsAccountMapper {

        public static WindowsAccountDetails ToWindowsAccountDetails(this WindowsAccount windowsAccount) {
            return new WindowsAccountDetails() {
                Username = windowsAccount.Username
            };
        }

        public static WindowsAccount ToWindowsAccount(this WindowsAccountDetails windowsAccountDetails) {
            return new WindowsAccount() {
                Username = windowsAccountDetails.Username
            };
        }

    }
}
