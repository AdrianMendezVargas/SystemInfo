using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemInfo.Models.Data;
using SystemInfo.Models.Domain;

namespace SystemInfo.Wpf.Data {
    public class OfflineApplicationDbContext : ApplicationDbContext {

        public OfflineApplicationDbContext(): base() {}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseSqlite("Data source= Data\\SystemInfoLocalDB.db");
            base.OnConfiguring(optionsBuilder);
        }

    }
}
