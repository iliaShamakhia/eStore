using GameStoreDAL.Data;
using GameStoreDAL.Entities;
using GameStoreDAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStoreDAL.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly GameStoreDbContext _context;

        public OrderRepository(GameStoreDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Order entity)
        {
            await _context.Orders.AddAsync(entity);
        }

        public void Delete(Order entity)
        {
            _context.Orders.Remove(entity);
        }

        public async Task DeleteByIdAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null) return;

            _context.Orders.Remove(order);
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Game)
                .ToListAsync();
        }

        public async Task<Order> GetByIdAsync(int id)
        {
            var orders = await GetAllAsync();

            var order = orders.FirstOrDefault(o => o.Id == id);

            if (order == null) return null;

            return order;
        }

        public async Task<Order> GetByUserIdAsync(string id)
        {
            var orders = await GetAllAsync();

            var order = orders.FirstOrDefault(o => o.UserId == id);

            if (order == null) return null;

            return order;
        }

        public void Update(Order entity)
        {
            _context.Orders.Update(entity);
        }
    }
}
