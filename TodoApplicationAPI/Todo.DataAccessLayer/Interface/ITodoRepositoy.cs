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

        Task<IEnumerable<TodoGetAll>> TodoGetAllRepo();

        Task<bool> DeleteTodoRepo(int id);

        Task<bool> UpdateTodoRepo(int id, TodoUpdateDTO model);

        Task<bool> PatchTodoRepo(int id, UpdateTodoStatusIdDTO dto);

        Task<GetOneDTO> GetOneRepo(int id);
    }
}
