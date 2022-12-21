using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStoreBLL.Models
{
    public class GameModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public double Price { get; set; }
        public ICollection<CommentModel> Comments { get; set; }
        public ICollection<GenreModel> Genres { get; set; }
    }
}
