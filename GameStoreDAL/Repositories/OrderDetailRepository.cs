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
    public class OrderDetailRepository : IOrderDetailRepository
    {
        private readonly GameStoreDbContext _context;

        public OrderDetailRepository(GameStoreDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(OrderDetail entity)
        {
            await _context.OrderDetails.AddAsync(entity);
        }

        public void Delete(OrderDetail entity)
        {
            _context.OrderDetails.Remove(entity);
        }

        public async Task DeleteByIdAsync(int id)
        {
            var orderDetail = await _context.OrderDetails.FindAsync(id);

            if (orderDetail == null) return;

            _context.OrderDetails.Remove(orderDetail);
        }

        public async Task<IEnumerable<OrderDetail>> GetAllAsync()
        {
            return await _context.OrderDetails
                .Include(od => od.Game)
                .Include(od => od.Order)
                .ToListAsync();
        }

        public async Task<OrderDetail> GetByIdAsync(int id)
        {
            var orderDetails = await GetAllAsync();

            var orderDetail = orderDetails.FirstOrDefault(od => od.Id == id);

            if (orderDetail == null) return null;

            return orderDetail;
        }

        public void Update(OrderDetail entity)
        {
            _context.OrderDetails.Update(entity);
        }
    }
}
