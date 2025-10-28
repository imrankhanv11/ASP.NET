using EmployeeOnboarding.DataAccessLayer.Data;
using EmployeeOnboarding.DataAccessLayer.Interfaces;
using EmployeeOnboarding.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeOnboarding.DataAccessLayer.Repositories
{
    public class MetaLogRepository : IMetaLogRepository
    {
        private readonly EmployeeContext _dbContext;

        public MetaLogRepository(EmployeeContext dbContext)
        {
            _dbContext = dbContext;   
        }

        public async Task<MetaLog> MetLogRepo(MetaLog metaLog)
        {
            _dbContext.MetaLogs.Add(metaLog);

            await _dbContext.SaveChangesAsync();
            
            return metaLog;
        }
    }
}