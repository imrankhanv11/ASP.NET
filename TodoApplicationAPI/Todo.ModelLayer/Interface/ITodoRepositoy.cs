using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.ModelLayer.DTO.Request;
using Todo.ModelLayer.DTO.Response;

namespace Todo.DataAccessLayer.Interface
{
    public interface ITodoRepositoy
    {
        Task<TodoModel> TodoAddRepo(TodoModel todo);

        Task<IEnumerable<TodoModel>> TodoGetAllRepo(int userId);

        Task<bool> DeleteTodoRepo(int id);

        Task<bool> UpdateTodoRepo(int id, TodoUpdateDTO model, int Uid);

        Task<bool> PatchTodoRepo(int id, UpdateTodoStatusIdDTO dto);

        Task<GetOneDTO> GetOneRepo(int id, int UId);

        Task<IEnumerable<TodoModel>> SearchRepo(int? status, int? cat, int UId);
    }
}
