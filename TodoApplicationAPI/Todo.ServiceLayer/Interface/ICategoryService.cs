using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.ModelLayer.DTO.Request;
using Todo.ModelLayer.DTO.Response;

namespace Todo.ServiceLayer.Interface
{
    public interface ICategoryService
    {
        Task<AddCatResponceDTO> AddCatService(AddCatDTO dto);
    }
}
