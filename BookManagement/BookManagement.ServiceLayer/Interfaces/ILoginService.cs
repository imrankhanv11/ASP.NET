using BookManagement.ServiceLayer.DTO.Login.Request;
using BookManagement.ServiceLayer.DTO.Login.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManagement.ServiceLayer.Interfaces
{
    public interface ILoginService
    {
        Task<bool> RegisterUserService(string userName, string password);

        Task<LoginResponseDTO> LoginUserService(LoginReqDTO userLogin);

        Task<LoginResponseDTO> RefreshTokenService(string token);

        Task<string> GenerateJWTToken(string RoleName, int UserId, bool isAccestoken);
    }
}
