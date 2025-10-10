using AutoMapper;
using ECommerce.DataAccessLayer.Inteface;
using ECommerce.DataAccessLayer.Models;
using ECommerce.ServiceLayer.DTO.Customers.Request;
using ECommerce.ServiceLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.ServiceLayer.Service
{
    public class CustomerService : ICustomerService
    {
        public readonly IGenericRepo<Product> _productRepo;
        public readonly ICustomerRepo _customerRepo;
        public readonly IMapper _mapper;
        public CustomerService(IGenericRepo<Product> Productrepo, ICustomerRepo customerrepo, IMapper mapper)
        {
            _customerRepo = customerrepo;
            _productRepo = Productrepo;
            _mapper = mapper;
        }

        public async Task<bool> AddtoCartService(CartAddDTO dto, int id)
        {

            var cart = await _customerRepo.checkExitsCartRepo(id);

            if (cart == null)
            {

                var value = _mapper.Map<Cart>(dto);

                value.UserId = id;

                var output = await _customerRepo.AddtoCartRepo(value);
            }
            else
            {
                foreach (var item in dto.CartItems)
                {
                    var existingItem = cart.CartItems
                        .FirstOrDefault(ci => ci.ProductId == item.ProductId);

                    if (existingItem != null)
                    {
                        existingItem.Quantity += item.Quantity;
                    }
                    else
                    {
                        cart.CartItems.Add(new CartItem
                        {
                            ProductId = item.ProductId,
                            Quantity = item.Quantity
                        });
                    }
                }

                await _customerRepo.UpdateCartRepo(cart);
            }

            return true;
        }

        public async Task<bool> OrderProductsService(OrderRequestDTO dto, int id)
        {
            dto.UserId = id;
            dto.OrderDate = DateTime.Now;
            dto.Status = "Ordered";

            var products = await _productRepo.GetAllAsync();

            var order = new Order
            {
                UserId = dto.UserId,
                OrderDate = dto.OrderDate,
                Status = dto.Status,
                OrderItems = dto.OrderItems.Select(i =>
                {
                    var product = products.First(p => p.ProductId == i.ProductId);
                    return new OrderItem
                    {
                        ProductId = i.ProductId,
                        Quantity = i.Quantity,
                        Price = product.Price * i.Quantity
                    };
                }).ToList()
            };

            // Calculate total
            order.TotalAmount = order.OrderItems.Sum(i => i.Price);

            var output = await _customerRepo.OrderProductsRepo(order);

            return output;
        }
    }
}
