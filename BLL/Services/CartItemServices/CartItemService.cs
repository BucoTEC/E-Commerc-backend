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

        public async Task<CartItem> CreateCartItem(CreateUpdateCartItemDto cartItemDto)
        {
            var cartItem = await _unitOfWork.CartItem.CreateCartItem(cartItemDto);

            await _unitOfWork.CompleteAsync();

            return cartItem;
        }

        public async Task<bool> DeleteCartItem(int id)
        {
            await _unitOfWork.CartItem.DeleteCartItem(id);

            await _unitOfWork.CompleteAsync();

            return true;
        }

        public async Task<bool> DeleteCartItemByUser(int id, string userId)
        {
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
            Console.WriteLine(userId);
            return await _unitOfWork.CartItem.GetCartItemsByUser(userId);
        }

        public async Task<CartItem> UpdateCartItem(CreateUpdateCartItemDto cartItemDto, int productId, string userId)
        {
            var cartItem = await _unitOfWork.CartItem.UpdateCartItem(cartItemDto, productId, userId);

            await _unitOfWork.CompleteAsync();

            return cartItem;
        }
    }
}