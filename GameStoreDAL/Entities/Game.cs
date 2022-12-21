using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStoreDAL.Entities
{
    public class Game : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public double Price { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Genre> Genres { get; set; }
    }
}
