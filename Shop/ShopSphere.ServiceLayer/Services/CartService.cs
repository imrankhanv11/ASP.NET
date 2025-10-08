using AutoMapper;
using Microsoft.Extensions.Logging;
using ShopSphere.DataAccessLayer.Interface;
using ShopSphere.DataAccessLayer.Models;
using ShopSphere.ServiceLayer.DTO.Cart.Request;
using ShopSphere.ServiceLayer.DTO.Cart.Response;
using ShopSphere.ServiceLayer.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace ShopSphere.ServiceLayer.Services
{
    public class CartService : ICartService
    {

        private readonly ILogger<CartService> _logger;
        private readonly ICartRepo _repo;
        private readonly IMapper _mapper;

        public CartService(ILogger<CartService> logger, ICartRepo repo, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
            _repo = repo;
        }

        public async Task<int> AddtoCartService(AddtoCartDTO dto, int UserId)
        {
            // Validations
            if(dto.CartItems.Any(s=> s.Quantity <= 0))
            {
                throw new ValidationException("Card Quantity Can't be Less than or Equl to Zero");
            }

            // Convertion for Validations on Quntity <= Stock
            IEnumerable<CartItem> ProuductsIDs  = dto.CartItems.Select(s=> new CartItem
            {
                ProductId = s.ProductId,
                Quantity = s.Quantity
            });

            // Validations in DB
            await _repo.CheckProductQuanity(ProuductsIDs);
            _logger.LogInformation("Products and Quantity Satisfy the Conditions");

            // Check User already have Cart
            var cartCheck = await _repo.checkExitsCartRepo(UserId);

            // If not have Cart
            if (cartCheck == null)
            {
                _logger.LogInformation("User Not have Existing Cart");

                var value = _mapper.Map<Cart>(dto);

                value.UserId = UserId;

                // add to the cart
                var output = await _repo.AddtoCartRepo(value);
                _logger.LogInformation("Cart Added Succesfully");

                // reduce the quntity
                await _repo.ReduceQuantity(ProuductsIDs);
                _logger.LogInformation("Quantity Reduced succesfully");

                return output.CartId;

            }
            // If User have Cart with some products
            else
            {
                // for each product increase the product quanity in stock
                foreach (var item in dto.CartItems)
                {
                    var OldItems = cartCheck.CartItems
                        .FirstOrDefault(ci => ci.ProductId == item.ProductId);

                    if (OldItems != null)
                    {
                        OldItems.Quantity += item.Quantity;
                    }
                    else
                    {
                        cartCheck.CartItems.Add(new CartItem
                        {
                            ProductId = item.ProductId,
                            Quantity = item.Quantity
                        });
                    }
                }

                // update the cart
                var output = await _repo.UpdateCartRepo(cartCheck);
                _logger.LogInformation("Cart Updated Succesfully");


                // reduce the Product table 
                await _repo.ReduceQuantity(ProuductsIDs);
                _logger.LogInformation("Quantity Reduced succesfully");

                return output.CartId;
            }
        }

        public async Task<IEnumerable<GetAllCart>> GetAllCartService(int UserId)
        {
            var CartItems = await _repo.GetAllCart(UserId);
            _logger.LogInformation("All Cart Items get Succesfully");

            var output = _mapper.Map<IEnumerable<GetAllCart>>(CartItems);

            // Calculating the Total Amount
            foreach (var item in output)
            {
                item.SubTotal = item.Price * item.Quantity;
            }
            _logger.LogInformation("SubTotal Calculated");

            return output;
        }

        public async Task<decimal> GetTotalAmount(IEnumerable<GetAllCart> cart)
        {
            decimal output = 0;

            foreach (var cartItem in cart)
            {
                output += cartItem.SubTotal;
            }
            _logger.LogInformation("Grand Total Calculated");

            return output;
        }
    }
}
