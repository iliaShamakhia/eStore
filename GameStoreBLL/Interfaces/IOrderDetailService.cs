using GameStoreBLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStoreBLL.Interfaces
{
    public interface IOrderDetailService
    {
        Task<IEnumerable<OrderDetailModel>> GetAllByOrderIdAsync(int id);
        Task AddAsync(OrderDetailModel model);
        Task IncreaseQuantity(int id);
        Task DecreaseQuantity(int id);
        Task DeleteAsync(int modelId);
    }
}
