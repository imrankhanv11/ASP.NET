using LibraryManagement.Service.DTO.Account.Request;
using LibraryManagement.Service.DTO.Account.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Service.Interface
{
    public interface IAccountService
    {
        Task<UserRegisterResponseDTO> UserRegisterService(UserRegisterDTO dto);

        Task<UserLoginResponstDTO> UserLoginService(UserLoginRequestDTO dto);
        string GenerateJWTToken(int userId, string role, bool isAccessToken);

        Task<UserLoginResponstDTO> RefreshTokenService(string refreshToken);
    }
}
