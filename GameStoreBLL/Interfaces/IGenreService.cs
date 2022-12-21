using GameStoreBLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStoreBLL.Interfaces
{
    public interface IGenreService
    {
        Task<IEnumerable<GenreModel>> GetAllAsync();
        Task<GenreModel> GetByIdAsync(int id);
        Task AddAsync(GenreModel model);
        Task UpdateAsync(GenreModel model);
        Task DeleteAsync(int modelId);
    }
}
