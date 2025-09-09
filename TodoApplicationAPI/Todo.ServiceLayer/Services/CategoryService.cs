using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.DataAccessLayer;
using Todo.DataAccessLayer.Interface;
using Todo.ModelLayer.DTO.Request;
using Todo.ModelLayer.DTO.Response;
using Todo.ServiceLayer.Interface;
using Todo.ModelLayer.ValidationResponse;

namespace Todo.ServiceLayer.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repository;

        public CategoryService(ICategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<ResultSet<AddCatResponceDTO>> AddCatService(AddCatDTO dto)
        {
            if (dto.name.Length >= 10)
                return new ResultSet<AddCatResponceDTO>
                {
                    Success = false,
                    ErrorMessage = "Name Can't More then 9 Char",
                    Field = "Name"
                };

            var cat = new Category
            {
                Name = dto.name
            };

            var value = await _repository.AddCatRepo(cat);

            return new ResultSet<AddCatResponceDTO>
            {
                Success = true,
                Data = value
            };
        }
    }
}
