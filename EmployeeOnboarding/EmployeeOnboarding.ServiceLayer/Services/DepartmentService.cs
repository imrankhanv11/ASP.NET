using EmployeeOnboarding.DataAccessLayer.Interfaces;
using EmployeeOnboarding.ServiceLayer.DTO.Department.Response;
using EmployeeOnboarding.ServiceLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeOnboarding.ServiceLayer.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepo _repo;


        public DepartmentService(IDepartmentRepo repo)
        {
            _repo = repo;
        }
        public async Task<IEnumerable<DepartmentResponseDTO>> GetAllDepartment()
        {
            var departments = await _repo.GetDepartmentsRepo();

            var responseModel = departments.Select(s=> new DepartmentResponseDTO
            {
                Id = s.Id,
                DepartmentName = s.DepartmentName
            }).ToList();

            return responseModel;
        }

        public async Task<IEnumerable<LocationResponseDTO>> GetAllLocations()
        {
            var locations = await _repo.GetAllLocations();

            var responseModel = locations.Select(s => new LocationResponseDTO
            {
                Id = s.Id,
                LocationName = s.LocationName
            }).ToList();

            return responseModel;
        }

        public async Task<IEnumerable<RoleResponseDTO>> GetAllRole(int deparmentId)
        {
            bool checkExits = await _repo.CheckDepartmentExits(deparmentId);

            if (!checkExits)
            {
                throw new KeyNotFoundException("Departmnet Id not found");
            }

            var roles = await _repo.GetAllRolesRepo(deparmentId);

            var responseModel = roles.Select(s => new RoleResponseDTO
            {
                Id = s.Id,
                RoleName = s.RoleName
            }).ToList();

            return responseModel;
        }
    }
}
