using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStoreDAL.Interfaces
{
    public interface IUnitOfWork
    {
        public IGameRepository GameRepository { get; }
        public ICommentRepository CommentRepository { get; }
        public IOrderRepository OrderRepository { get; }
        public IOrderDetailRepository OrderDetailRepository { get; }
        public IGenreRepository GenreRepository { get; }
        Task SaveAsync();
    }
}
