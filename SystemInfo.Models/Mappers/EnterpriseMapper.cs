using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemInfo.Models.Domain;
using SystemInfo.Shared.Models;
using SystemInfo.Shared.Requests;

namespace SystemInfo.Models.Mappers {
    public static class EnterpriseMapper {

        public static Enterprise ToEnterprise(this CreateEnterpriseRequest enterpriseRequest) {
            return new Enterprise() {
                RNC = enterpriseRequest.RNC,
                Name = enterpriseRequest.Name
            };
        }

        public static EnterpriseDetails ToEnterpriseDetails(this Enterprise enterprise) {
            return new EnterpriseDetails() {
                RNC = enterprise.RNC ,
                Name = enterprise.Name ,
                CreatedOn = enterprise.CreatedOn
            };
        }

        public static List<EnterpriseDetails> ToListEnterpriseDetails(this List<Enterprise> enterpriseList) {
            var enterpriseDetailsList = new List<EnterpriseDetails>();
            enterpriseList.ForEach((enterprise) => {
                enterpriseDetailsList.Add(enterprise.ToEnterpriseDetails());
            });
            return enterpriseDetailsList;
        }

        public static CreateEnterpriseRequest ToCreateEntepriseRequest(this Enterprise enterprise) {
            return new CreateEnterpriseRequest() {
                RNC = enterprise.RNC ,
                Name = enterprise.Name ,
            };
        }

    }
}
