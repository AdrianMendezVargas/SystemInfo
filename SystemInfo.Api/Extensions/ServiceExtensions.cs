using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SystemInfo.Models.Data;
using SystemInfo.Repository;
using SystemInfo.Services;

namespace SystemInfo.Api.Extensions {
    public static class ServiceExtensions {

        public static void AddApplicationDbContext(this IServiceCollection services , IConfiguration configuration) {
            services.AddDbContext<ApplicationDbContext>(options => {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection") , sqlOptions => {
                    sqlOptions.MigrationsAssembly("SystemInfo.Api");
                });
            });
        }

        public static void AddUnitOfWork(this IServiceCollection services ) {
            services.AddScoped<IUnitOfWork , EfUnitOfWork>();
        }

        public static void AddBussinessServices(this IServiceCollection services) {
            services.AddScoped<ISystemSpecsService , SystemSpecsService>();
            services.AddScoped<IEnterpriseService , EnterpriseService>();

        }

    }
}
