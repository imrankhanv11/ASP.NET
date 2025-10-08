using ECommerce.ServiceLayer.DTO.Authentications.Request;
using ECommerce.ServiceLayer.DTO.Authentications.Response;
using Microsoft.AspNetCore.Identity.Data;
using ShopSphere.ServiceLayer.DTO.Authentication.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopSphere.ServiceLayer.Interface
{
    public interface IAuthService 
    {
        Task<int> RegisterUserService(RegisterUserDTO newUser);

        Task<string> GenereteJWTTokenService(int id, string RoleName, bool IsAccessToken);

        Task<LoginResponseDTO> LoginService(LoginRequestDTO dto);

        Task<LoginResponseDTO> RefreshTokenService(string refreshToken);
    }
}
