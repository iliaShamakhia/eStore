using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStoreBLL.Models
{
    public class OrderModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public ICollection<OrderDetailModel> OrderDetails { get; set; }
    }
}
