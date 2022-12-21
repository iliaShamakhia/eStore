using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStoreBLL.Models
{
    public class CommentModel
    {
        public int Id { get; set; }
        public string Body { get; set; }
        public bool IsDeleted { get; set; }
        public int GameId { get; set; }
        public string UserId { get; set; }
        //public UserModel User { get; set; }
        public string Username { get; set; }
        public DateTime DateAdded { get; set; }
        public int? ParentCommentId { get; set; }
        public ICollection<CommentModel> ChildComments { get; set; }
    }
}
