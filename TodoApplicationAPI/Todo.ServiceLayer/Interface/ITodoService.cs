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
        Task<ResultSet<TodoAddResponseDTO>> TodoAddservice(TodoAddDTO dto);

        Task<IEnumerable<TodoGetAll>> GetAllService();

        Task<bool> DeleteTodoService(int id);

        Task<bool> UpdateTodoService(int id, TodoUpdateDTO dto);

        Task<bool> PatchTodoService(int id, UpdateTodoStatusIdDTO dto);

        Task<GetOneDTO> GetOneService(int id);

        Task<IEnumerable<TodoGetAll>> SearchService(int? status, int? cat);
    }
}
