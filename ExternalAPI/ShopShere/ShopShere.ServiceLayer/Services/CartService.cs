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
        private readonly IProductRepo _productRepo;

        public CartService(ILogger<CartService> logger, ICartRepo repo, IMapper mapper, IProductRepo productRepo)
        {
            _logger = logger;
            _mapper = mapper;
            _repo = repo;
            _productRepo = productRepo;
        }

        public async Task<CartItemDTO> AddtoCartService(AddCartItemsDTO dto, int UserId)
        {
            // Validations
            if (dto.Quantity <= 0)
                throw new ValidationException("Cart Quantity can't be less than or equal to zero");

            var checkQuantity = await _repo.CheckProductQuanity(dto.ProductId);
            if (!checkQuantity)
                throw new ValidationException("Product is out of stock");

            _logger.LogInformation("Products and quantity satisfy the conditions");

            var cartCheck = await _repo.checkExitsCartRepo(UserId);
            CartItem addedOrUpdatedItem;

            if (cartCheck == null)
            {
                _logger.LogInformation("User does not have an existing cart");

                var newCartItem = new CartItem
                {
                    ProductId = dto.ProductId,
                    Quantity = dto.Quantity
                };

                var value = new Cart
                {
                    UserId = UserId,
                    CartItems = new List<CartItem> { newCartItem }
                };

                var output = await _repo.AddtoCartRepo(value);
                _logger.LogInformation("Cart added successfully");

                await _repo.ReduceQuantity(dto.ProductId);
                _logger.LogInformation("Quantity reduced successfully");

                addedOrUpdatedItem = value.CartItems.First();
            }
            else
            {
                var existingItem = cartCheck.CartItems.FirstOrDefault(ci => ci.ProductId == dto.ProductId);

                if (existingItem != null)
                {
                    existingItem.Quantity += dto.Quantity;
                    addedOrUpdatedItem = existingItem;
                }
                else
                {
                    var newItem = new CartItem
                    {
                        ProductId = dto.ProductId,
                        Quantity = dto.Quantity
                    };
                    cartCheck.CartItems.Add(newItem);
                    addedOrUpdatedItem = newItem;
                }

                var output = await _repo.UpdateCartRepo(cartCheck);
                _logger.LogInformation("Cart updated successfully");

                await _repo.ReduceQuantity(dto.ProductId);
                _logger.LogInformation("Quantity reduced successfully");
            }

            // Map CartItem to DTO to include productName, price, subTotal
            var product = await _productRepo.GetProductById(addedOrUpdatedItem.ProductId);

            return new CartItemDTO
            {
                CartItemId = addedOrUpdatedItem.CartItemId,
                ProductId = addedOrUpdatedItem.ProductId,
                ProductName = product.Name,
                Price = product.Price,
                Quantity = addedOrUpdatedItem.Quantity,
                SubTotal = product.Price * addedOrUpdatedItem.Quantity
            };
        }

        public async Task<bool> cartDeleteDTO(CartDeleteRequest dto)
        {
            await _repo.DeleteCartItem(dto.CartItemID);

            var product = await _productRepo.GetProductById(dto.ProductId);

            product.Stock += dto.Quantity;

            await _productRepo.UpdateQuanity(product);

            return true;
        }

        public async Task<IEnumerable<GetAllCart>> GetAllCartService(int userId)
        {
            // Get cart items for the user
            var cartItems = await _repo.GetAllCart(userId);

            _logger.LogInformation("All Cart Items fetched successfully");

            // Map manually to DTO to avoid cycles
            var output = cartItems.Select(ci => new GetAllCart
            {
                CartItemId = ci.CartItemId,
                ProductId = ci.ProductId,
                ProductName = ci.Product.Name,  // get from navigation property
                Price = ci.Product.Price,
                Quantity = ci.Quantity,
                SubTotal = ci.Product.Price * ci.Quantity
            }).ToList();

            _logger.LogInformation("SubTotal Calculated for each cart item");

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

        public async Task<CartItem> UpdateCartItemQuantity(int cartItemId, UpdateCartItemDTO dto)
        {
            var item = await _repo.GetCartItemById(cartItemId);
            if (item == null)
                throw new KeyNotFoundException("Cart item not found");

            // Increment
            if (dto.Increment.HasValue && dto.Increment.Value > 0)
            {
                bool available = await _repo.CheckProductQuanity(item.ProductId);
                if (!available)
                    throw new ValidationException("Not enough stock");

                item.Quantity += dto.Increment.Value;
                await _repo.ReduceQuantity(item.ProductId);
            }

            // Decrement
            if (dto.Decrement.HasValue && dto.Decrement.Value > 0)
            {
                item.Quantity -= dto.Decrement.Value;
                await _repo.IncreaseProductQuantity(item.ProductId);

                if (item.Quantity < 1)
                {
                    await _repo.DeleteCartItem(cartItemId);
                    return null;
                }
            }

            await _repo.UpdateCartItem(item);
            return item;
        }

        public async Task<CartItemDTO> IncreaseQuantity(int cartItemId)
        {
            var cartItem = await _repo.GetCartItemById(cartItemId);
            if (cartItem == null) return null;

            var product = await _productRepo.GetProductById(cartItem.ProductId);    

            if(product.Stock == 0)
            {
                throw new ValidationException("Product is Out of Stock");
            }

            cartItem.Quantity += 1;
            await _repo.UpdateCartItem(cartItem);

            product.Stock -= 1;

            await _productRepo.UpdateQuanity(product);

            return new CartItemDTO
            {
                CartItemId = cartItemId,
                Quantity = cartItem.Quantity,
                ProductId = cartItem.ProductId,
                ProductName = cartItem.Product.Name,
                Price = cartItem.Product.Price,
                SubTotal = cartItem.Quantity * cartItem.Product.Price
            };
        }

        public async Task<CartItemDTO> DecreaseQuantity(int cartItemId)
        {
            var cartItem = await _repo.GetCartItemById(cartItemId);
            if (cartItem == null) return null;


            var product = await _productRepo.GetProductById(cartItem.ProductId);

            product.Stock += 1;

            await _productRepo.UpdateQuanity(product);

            if (cartItem.Quantity > 1)
            {
                cartItem.Quantity -= 1;
                await _repo.UpdateCartItem(cartItem);
            }
            else
            {
                await _repo.DeleteCartItem(cartItemId);
                return new CartItemDTO
                {
                    CartItemId = cartItemId,
                    Quantity = 0
                };
            }

            return new CartItemDTO
            {
                CartItemId = cartItemId,
                Quantity = cartItem.Quantity,
                ProductId = cartItem.ProductId,
                ProductName = cartItem.Product.Name,
                Price = cartItem.Product.Price,
                SubTotal = cartItem.Quantity * cartItem.Product.Price
            };
        }
    }
}