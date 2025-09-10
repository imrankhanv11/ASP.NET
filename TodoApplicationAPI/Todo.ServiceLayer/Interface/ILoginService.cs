using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.ModelLayer.DTO.Request;
using Todo.ModelLayer.DTO.Response;

namespace Todo.ServiceLayer.Interface
{
    public interface ILoginService
    {
        Task<LoginResponseDTO> GenerateJwtToken(string username, string role);

        Task<LoginAuthenticationDTO> CheckPassWordUserService(LoginDTO dto);

        string GenerateRefreshToken();

        Task AddRefresh(string token, string email,string roll);

        Task<LoginResponseDTO> GenerateNewToken(string token);
    }
}
