using EmployeeOnboarding.DataAccessLayer.Data;
using EmployeeOnboarding.DataAccessLayer.Interfaces;
using EmployeeOnboarding.DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeOnboarding.DataAccessLayer.Repositories
{
    public class HodRepository : IHodRepositroy
    {

        private readonly EmployeeContext _dbContext;

        public HodRepository(EmployeeContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Hod>> GetDeparmentHodNotify(int departmentId)
        {
            var hodList = await _dbContext.Hods.Where(s => s.DepartmentId == departmentId).ToListAsync();

            return hodList;
        }
    }
}
