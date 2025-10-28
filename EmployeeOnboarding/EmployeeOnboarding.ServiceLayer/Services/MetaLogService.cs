using EmployeeOnboarding.DataAccessLayer.Interfaces;
using EmployeeOnboarding.DataAccessLayer.Models;
using EmployeeOnboarding.ServiceLayer.DTO.MetaLog.Request;
using EmployeeOnboarding.ServiceLayer.DTO.MetaLog.Response;
using EmployeeOnboarding.ServiceLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeOnboarding.ServiceLayer.Services
{
    public class MetaLogService : IMetaLogService
    {
        private readonly IMetaLogRepository _repo;

        public MetaLogService(IMetaLogRepository repo)
        {
            _repo = repo;
        }

        public enum Status
        {
            Success, Error, 
        }

        public enum Department
        {
            IT = 1, HR = 2, Finance = 3, Marketing = 4
        }


        public async Task<MetaLogResponseDTO> MetaLogServiceCall(MetaLogRequestDTO dto)
        {
            if(dto.DepartmentId == (int)Department.IT || dto.DepartmentId == (int)Department.HR)
            {
                throw new ValidationException("MetaLog Employee Should be either Finace or Marketing");
            }


            var metaLogModel = new MetaLog
            {
                EmployeeId = dto.EmployeeId,
                DepartmentId = dto.DepartmentId,
                RoleId = dto.RoleId,
                JoiningDate = dto.JoiningDate
            };

            var metLogSave = await _repo.MetLogRepo(metaLogModel);

                var responseModel = new MetaLogResponseDTO
                {
                    Id = metaLogModel.Id,
                    Status = Status.Success.ToString(),
                    Message = "Employee data Successfully added to metalog and Processed for payroll setup",
                    ProcessedBy = metLogSave.DepartmentId == (int)Department.Finance ? "Finance Api" : "MakettingApi",
                    ProcessedDate = DateTime.UtcNow
                };

                return responseModel;
        }
    }
}
