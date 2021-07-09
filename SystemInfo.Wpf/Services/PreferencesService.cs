using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemInfo.Wpf.Data;

namespace SystemInfo.Wpf.Services {
    public class PreferencesService {
        private readonly OfflineApplicationDbContext _dbContext;

        public PreferencesService(OfflineApplicationDbContext dbContext) {
            _dbContext = dbContext;
        }

        public async Task<PreferencesKeyValues> Get(string key) {
            var value = await _dbContext.Preferences.FindAsync(key);
            if (value != null) {
                _dbContext.Entry(value).State = EntityState.Detached;
            }
            return value;
        }

        public async Task<bool> Insert(PreferencesKeyValues keyValues) {
            try {
                await _dbContext.Preferences.AddAsync(keyValues);
                return await _dbContext.SaveChangesAsync() > 0;
            } catch (Exception) {
                return false;
            }
        }

        public async Task<bool> Update(PreferencesKeyValues keyValues) {
            try {
                _dbContext.Preferences.Update(keyValues);
                return await _dbContext.SaveChangesAsync() > 0;
            } catch (Exception) {
                return false;
            }
        }

        public async Task<bool> Save(PreferencesKeyValues keyValues) {
            var value = await Get(keyValues.Key);

            return value == null
                ? await Insert(keyValues)
                : await Update(keyValues);
        }
    }
}
