using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStoreDAL.Entities
{
    public class Order : BaseEntity
    {
        public string UserId { get; set; }
        public User User { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
