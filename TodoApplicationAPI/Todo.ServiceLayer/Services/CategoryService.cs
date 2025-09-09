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

namespace Todo.ServiceLayer.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repository;

        public CategoryService(ICategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<AddCatResponceDTO> AddCatService(AddCatDTO dto)
        {
            var cat = new Category
            {
                Name = dto.name
            };

            return await _repository.AddCatRepo(cat);
        }
    }
}
