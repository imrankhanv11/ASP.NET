using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.ModelLayer.DTO.Request;
using Todo.ModelLayer.DTO.Response;
using Todo.ModelLayer.ValidationResponse;

namespace Todo.ServiceLayer.Interface
{
    public interface ITodoService
    {
        Task<ResultSet<TodoAddResponseDTO>> TodoAddservice(TodoAddDTO dto, int id);

        Task<IEnumerable<TodoGetAll>> GetAllService(int userID);

        Task<bool> DeleteTodoService(int id, int Uid);

        Task<bool> UpdateTodoService(int id, TodoUpdateDTO dto, int Uid);

        Task<bool> PatchTodoService(int id, UpdateTodoStatusIdDTO dto, int UId);

        Task<GetOneDTO> GetOneService(int id, int UId);

        Task<IEnumerable<TodoGetAll>> SearchService(int? status, int? cat, int UId);
    }
}
