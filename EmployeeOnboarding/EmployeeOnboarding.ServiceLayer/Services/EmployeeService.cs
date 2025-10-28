using EmployeeOnboarding.DataAccessLayer.Interfaces;
using EmployeeOnboarding.DataAccessLayer.Models;
using EmployeeOnboarding.ServiceLayer.DTO.Employee.Request;
using EmployeeOnboarding.ServiceLayer.DTO.Employee.Response;
using EmployeeOnboarding.ServiceLayer.DTO.Helper.Request;
using EmployeeOnboarding.ServiceLayer.Helper.Interface;
using EmployeeOnboarding.ServiceLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeOnboarding.ServiceLayer.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _repo;
        private readonly IHelperService _helperSerive;

        public EmployeeService(IEmployeeRepository repo, IHelperService helperService)
        {
            _repo = repo;
            _helperSerive = helperService;
        }

        public enum Status
        {
            Pending, Verified, 
        }

        public async Task<EmployeeOnboardingResponseDTO> EmployeeOnboardingService(EmployeeOnboardRequestDTO empDto)
        {
            // Validations

            //FullName
            if(empDto.FirstName.Trim().Length < 3)
            {
                throw new ValidationException("First Name Atleast Need 3 Letters");
            }

            //LastName
            if(empDto.LastName.Trim().Length < 3)
            {
                throw new ValidationException("Last Name Atleast Need 3 Letters");
            }

            bool empEmailExits = await _repo.EmployeeEmailIdExits(empDto.Email);

            // Check Email alredy Exits
            if (empEmailExits)
            {
                throw new ValidationException("Email Id Already Exits");
            }

            // Experience
            if(empDto.Experience <= 0)
            {
                throw new ValidationException("Experience Can't be Negative");
            }

            // Joining Date
            if(empDto.JoiningDate > DateOnly.FromDateTime(DateTime.Now))
            {
                throw new ValidationException("Joining Date Can't be in Future");
            }

            // CTC
            if(empDto.Ctc <= 0)
            {
                throw new ValidationException("CTC can't be Zero or Negative");
            }

            // Probagation Date
            var probationEndDate = empDto.JoiningDate.AddMonths(6);

            int submisstioId = await _repo.EmployeeSubmisstionId();

            // Model Convertion
            var newEmployee = new Employee
            {
                FirstName = empDto.FirstName,
                SubmissionId = submisstioId + 1,
                LastName = empDto.LastName,
                MiddleName = empDto.MiddleName,
                Email = empDto.Email,
                PhoneNumber = empDto.PhoneNumber,
                DepartmentId = empDto.DepartmentId,
                RoleId = empDto.RoleId,
                LocationId = empDto.LocationId,
                Experience = empDto.Experience,
                JoiningDate = empDto.JoiningDate,
                Ctc = empDto.Ctc,
                ProbationEndDate = probationEndDate,
                Status = Status.Pending.ToString()
            };

            var onBoardedEmployee = await _repo.EmployeeOnboardRepo(newEmployee);

            // sending Email

            var request = new NotifyHodRequestDTO
            {
                Id = onBoardedEmployee.Id,
                FullName = onBoardedEmployee.FirstName + " " + onBoardedEmployee.MiddleName + " " + onBoardedEmployee.LastName,
                JoiningDate = onBoardedEmployee.JoiningDate,
                Roleid = onBoardedEmployee.RoleId
            };

            await _helperSerive.NotifyHodHelperService(onBoardedEmployee.DepartmentId, request);

            // Model Convertion
            var responseModel = new EmployeeOnboardingResponseDTO
            {
                Id = onBoardedEmployee.Id,
                SubmissionId = onBoardedEmployee.SubmissionId,
                FirstName = onBoardedEmployee.FirstName,
                LastName = onBoardedEmployee.LastName,
                MiddleName = onBoardedEmployee.MiddleName,
                Email = onBoardedEmployee.Email,
                PhoneNumber = onBoardedEmployee.PhoneNumber,
                DepartmentId = onBoardedEmployee.DepartmentId,
                RoleId = onBoardedEmployee.RoleId,
                JoiningDate = onBoardedEmployee.JoiningDate,
                ProbationEndDate = onBoardedEmployee.ProbationEndDate,
                Status = onBoardedEmployee.Status,
                LocationId = onBoardedEmployee.LocationId,
                Experience = onBoardedEmployee.Experience
            };

            return responseModel;

        }

    }
}
