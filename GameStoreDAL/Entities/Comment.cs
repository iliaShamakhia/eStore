using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStoreDAL.Entities
{
    public class Comment : BaseEntity
    {
        public string Body { get; set; }
        public bool IsDeleted { get; set; }
        public int GameId { get; set; }
        public Game Game { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public string Username { get; set; }
        public DateTime DateAdded { get; set; }
        public int? ParentCommentId { get; set; }
        public Comment ParentComment { get; set; }
        public ICollection<Comment> ChildComments { get; set; }
    }
}
