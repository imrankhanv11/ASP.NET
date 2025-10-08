using ShopSphere.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopSphere.DataAccessLayer.Interface
{
    public interface IProductRepo
    {
        Task<Product> AddNewProductRepo(Product product);

        Task<bool> CheckProductExitsRepo(string name);

        Task<IEnumerable<Product>> GetAllRepo();

        Task<Product> checkProductRepo(int id);

        Task UpdateQuanity(Product product);
    }
}
