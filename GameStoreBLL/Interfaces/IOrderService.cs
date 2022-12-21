using GameStoreBLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStoreBLL.Interfaces
{
    public interface IOrderService
    {
        Task<OrderModel> GetOrderByUserIdAsync(string userId);
        Task DeleteAsync(int id);
        Task AddAsync(OrderModel model);
    }
}
