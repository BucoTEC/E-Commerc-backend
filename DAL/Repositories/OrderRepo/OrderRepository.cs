using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using DAL.Data;
using DAL.Dtos;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.OrderRepo
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _context;
        public OrderRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Order> CreateOrder(CreateOrderDto createOrderDto, string userId)
        {
            var newOrder = new Order()
            {

                Address = createOrderDto.Address,
                City = createOrderDto.City,
                Country = createOrderDto.Country,
                PhoneNumber = createOrderDto.PhoneNumber,
                ReceiverName = createOrderDto.ReceiverName,
                AppUserId = userId

            };



            await _context.Orders.AddAsync(newOrder);


            return newOrder;
        }

        // TODO update return option for create order item
        public async Task<object> CreateOrderItem(OrderItem orderItem)
        {


            return await _context.OrderItems.AddAsync(orderItem);

        }

        public async Task<List<Order>> GetAllOrders()
        {
            return await _context.Orders.ToListAsync();
        }

        public async Task<List<Order>> GetOrdersByUser(string userId)
        {
            return await _context.Orders.Where(o => o.AppUserId == userId).ToListAsync();
        }

        public async Task<Order> GetSingleOrder(int id)
        {
            var order = await _context.Orders.Include(o => o.OrderItems).FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null)
            {
                throw new Exception("No order with this id");
            }

            return order;
        }
    }
}
