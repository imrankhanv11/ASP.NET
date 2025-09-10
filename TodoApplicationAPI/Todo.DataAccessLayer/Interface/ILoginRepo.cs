using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.ModelLayer.DTO.Request;
using Todo.ModelLayer.Models;

namespace Todo.DataAccessLayer.Interface
{
    public interface ILoginRepo
    {
        Task<User> CheckPasswordUser(LoginDTO dto);

        Task AddRefeshRepo(RefershToken dto);

        Task<RefershToken> CheckRefreshToken(string token);

        Task UpdateRevokeRefreshToken(RefershToken dto);
    }
}
