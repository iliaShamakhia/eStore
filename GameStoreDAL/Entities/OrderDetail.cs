using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStoreDAL.Entities
{
    public class OrderDetail : BaseEntity
    {
        public int Quantity { get; set; }

        public double Price { get; set; }

        public int GameId { get; set; }
        public Game Game { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; }
    }
}
