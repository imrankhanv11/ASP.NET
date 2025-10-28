using EmployeeOnboarding.DataAccessLayer.Interfaces;
using EmployeeOnboarding.ServiceLayer.DTO.Helper.Request;
using EmployeeOnboarding.ServiceLayer.Helper.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeOnboarding.ServiceLayer.Helper.HelperService
{
    public class HelperService : IHelperService
    {
        private readonly IHodRepositroy _hodRepo;

        public HelperService(IHodRepositroy hodRepo)
        {
            _hodRepo = hodRepo;
        }

        public async Task NotifyHodHelperService(int departmentId, NotifyHodRequestDTO requestDto)
        {
            var hodList = await _hodRepo.GetDeparmentHodNotify(departmentId);

            foreach (var hod in hodList)
            {
                var emailId = hod.Email;

                // Implementations of Sending Imail 
                Console.WriteLine("Sending Email to All Hod's in that Department");

            }
        }
    }
}
