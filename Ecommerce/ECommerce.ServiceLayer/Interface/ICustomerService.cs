using ECommerce.ServiceLayer.DTO.Customers.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.ServiceLayer.Interface
{
    public interface ICustomerService
    {
        Task<bool> OrderProductsService(OrderRequestDTO dto, int id);

        Task<bool> AddtoCartService(CartAddDTO dto, int id);
    }
}
