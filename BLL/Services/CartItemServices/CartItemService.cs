using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using BLL.Utils;
using DAL.Dtos;
using DAL.Entities;
using DAL.UOW;

namespace BLL.Services.CartItemServices
{
    public class CartItemService : ICartItemService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtHandler _jwtHandler;
        public CartItemService(IUnitOfWork unitOfWork, IJwtHandler jwtHandler)
        {
            _jwtHandler = jwtHandler;
            _unitOfWork = unitOfWork;
        }

        public async Task<CartItem> CreateCartItem(CreateUpdateCartItemDto cartItemDto, string token)
        {
            var product = await _unitOfWork.Product.GetProduct(cartItemDto.ProductId);

            if (product == null)
            {
                throw new Exception("No product with this id");
            }
            var userId = _jwtHandler.DecodeToken(token).UserId;

            var cartItems = await _unitOfWork.CartItem.GetCartItemsByUser(userId);


            if (product.AvailableAmount < cartItemDto.Quantity)
            {
                throw new Exception("Selected amount is over availble amount");
            }

            var existingCartItem = cartItems.FirstOrDefault(p => p.ProductId == product.ProductId);
            if (existingCartItem != null)
            {
                throw new Exception("Cart item with this product already exists");
            }



            var cartItem = await _unitOfWork.CartItem.CreateCartItem(cartItemDto, userId);

            await _unitOfWork.CompleteAsync();

            return cartItem;
        }

        public async Task<bool> DeleteCartItem(int id)
        {
            await _unitOfWork.CartItem.DeleteCartItem(id);

            await _unitOfWork.CompleteAsync();

            return true;
        }

        public async Task<bool> DeleteCartItemByUser(int id, string token)
        {
            var userId = _jwtHandler.DecodeToken(token).UserId;

            await _unitOfWork.CartItem.DeleteCartItemByUser(id, userId);

            await _unitOfWork.CompleteAsync();

            return true;

        }

        public async Task<CartItem> GetCartItem(int id)
        {
            return await _unitOfWork.CartItem.GetCartItem(id);
        }

        public async Task<List<CartItem>> GetCartItems()
        {
            return await _unitOfWork.CartItem.GetCartItems();
        }

        public async Task<List<CartItem>> GetCartItemsByUser(string token)
        {
            var userId = _jwtHandler.DecodeToken(token).UserId;

            return await _unitOfWork.CartItem.GetCartItemsByUser(userId);
        }

        public async Task<CartItem> UpdateCartItem(CreateUpdateCartItemDto cartItemDto, int productId, string token)
        {
            var product = await _unitOfWork.Product.GetProduct(cartItemDto.ProductId);

            if (product == null)
            {
                throw new Exception("No product with this id");
            }


            if (product.AvailableAmount < cartItemDto.Quantity)
            {
                throw new Exception("Selected quantity is larger than avalible amount");
            }

            var userId = _jwtHandler.DecodeToken(token).UserId;


            var cartItem = await _unitOfWork.CartItem.UpdateCartItem(cartItemDto, productId, userId);

            await _unitOfWork.CompleteAsync();

            return cartItem;
        }
    }
}
