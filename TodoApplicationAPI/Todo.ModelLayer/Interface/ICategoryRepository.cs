using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.ModelLayer.DTO.Response;

namespace Todo.DataAccessLayer.Interface
{
    public interface ICategoryRepository
    {
        Task<AddCatResponceDTO> AddCatRepo(Category cat);
    }
}
