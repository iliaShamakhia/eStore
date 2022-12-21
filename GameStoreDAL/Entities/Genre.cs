using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStoreDAL.Entities
{
    public class Genre : BaseEntity
    {
        public string Name { get; set; }
        public int? ParentGenreId { get; set; }
        public Genre ParentGenre { get; set; }
        public ICollection<Game> Games { get; set; }
    }
}
