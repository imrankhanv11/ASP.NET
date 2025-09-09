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
    public interface ICategoryService
    {
        Task<ResultSet<AddCatResponceDTO>> AddCatService(AddCatDTO dto);
    }
}
