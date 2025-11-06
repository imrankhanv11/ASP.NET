using ShopSphere.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopSphere.ServiceLayer.DTO.Authentication.Response
{
    public class LoginUserResponseDTO
    {
        public int UserId { get; set; }

        public string Name { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Role { get; set; }

        public DateOnly CreatedAt { get; set; }
    }
}
