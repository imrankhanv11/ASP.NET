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
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly EmployeeContext _dbContext;

        public EmployeeRepository(EmployeeContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> EmployeeEmailIdExits(string email)
        {
            var employeeEmailExitsCheck = await _dbContext.Employees.FirstOrDefaultAsync(s=> s.Email == email);

            if(employeeEmailExitsCheck != null)
            {
                return true;
            }

            return false;
        }

        public async Task<Employee> EmployeeOnboardRepo(Employee newEmployee)
        {
            _dbContext.Employees.Add(newEmployee);

            await _dbContext.SaveChangesAsync();

            return newEmployee;
        }

        // check

        public async Task<int> EmployeeSubmisstionId()
        {
            var employee =  _dbContext.Employees.OrderByDescending(s => s.SubmissionId).Take(1).First();

            return employee.SubmissionId;
        }
    }
}
