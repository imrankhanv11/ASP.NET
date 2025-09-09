using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.ModelLayer.DTO.Request;
using Todo.ModelLayer.DTO.Response;

namespace Todo.ServiceLayer.Interface
{
    public interface ITodoService
    {
        Task<TodoAddResponseDTO> TodoAddservice(TodoAddDTO dto);

        Task<IEnumerable<TodoGetAll>> GetAllService();

        Task<bool> DeleteTodoService(int id);

        Task<bool> UpdateTodoService(int id, TodoUpdateDTO dto);

        Task<bool> PatchTodoService(int id, UpdateTodoStatusIdDTO dto);

        Task<GetOneDTO> GetOneService(int id);
    }
}
