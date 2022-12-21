using GameStoreBLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStoreBLL.Interfaces
{
    public interface ICommentService 
    {
        Task<IEnumerable<CommentModel>> GetAllAsync();
        Task<CommentModel> GetByIdAsync(int id);
        Task AddAsync(CommentModel model);

        Task UpdateAsync(int id, CommentModel model);

        Task DeleteAsync(int modelId);
    }
}
