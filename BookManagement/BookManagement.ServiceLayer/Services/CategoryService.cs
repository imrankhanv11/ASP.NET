using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BookManagement.DataAccessLayer.Data;
using BookManagement.DataAccessLayer.Interfaces;
using BookManagement.DataAccessLayer.Models;
using BookManagement.ServiceLayer.DTO.Category.Request;
using BookManagement.ServiceLayer.DTO.Category.Response;
using BookManagement.ServiceLayer.Interfaces;


namespace BookManagement.ServiceLayer.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IGenericRepository<Category> _catRepo;
        private readonly IMapper _mapper;

        public CategoryService(IGenericRepository<Category> catRepo, IMapper mapper)
        {
            _catRepo = catRepo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetAllCategoryDTO>> GetAllCatService()
        {
            var value = await _catRepo.GetAllAsync();

            var result = _mapper.Map<IEnumerable<GetAllCategoryDTO>>(value);

            return result;
        }

        public async Task<GetOneCatDTO> GetOneCatService(int id)
        {
            var value = await _catRepo.GetByIdAsync(id);

            var output = _mapper.Map<GetOneCatDTO>(value);

            return output;
        }

        public async Task<int> AddNewCatService(AddCatDto dto)
        {
            var value = _mapper.Map<Category>(dto);

            var result = await _catRepo.AddAsync(value);

            return result.CategoryId;
        }

        public async Task<bool> DeleteCatService(int id)
        {
            var value = await _catRepo.GetByIdAsync(id);

            if(value == null) return false;

            await _catRepo.DeleteAsync(id);

            return true;
        }

    }
}
