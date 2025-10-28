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
    public class DepartmentRepo : IDepartmentRepo
    {

        private readonly EmployeeContext _dbContext;

        public DepartmentRepo(EmployeeContext dbContext)
        {
            _dbContext = dbContext;   
        }

        public async Task<bool> CheckDepartmentExits(int deparmentid)
        {
            var department =  _dbContext.Departments.Any(s=> s.Id == deparmentid);

            return department;
        }

        public async Task<IEnumerable<Location>> GetAllLocations()
        {
            var locations = await _dbContext.Locations.ToListAsync();

            return locations;
        }

        public async Task<IEnumerable<Role>> GetAllRolesRepo(int DepartmentId)
        {
            var roles = await _dbContext.Roles.Where(s=> s.DepartmentId == DepartmentId).ToListAsync();

            return roles;
        }

        public async Task<IEnumerable<Department>> GetDepartmentsRepo()
        {
            var departments = await _dbContext.Departments.ToListAsync();

            return departments;
        }
    }
}
