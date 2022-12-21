using GameStoreBLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStoreBLL.Interfaces
{
    public interface IGameService
    {
        Task<IEnumerable<GameModel>> GetAllAsync(); 
        Task<GameModel> GetByIdAsync(int id);
        Task AddAsync(GameModel model);
        Task UpdateAsync(GameModel model);
        Task DeleteAsync(int modelId);
        Task<IEnumerable<GameModel>> GetGamesByFilterAsync(FilterSearchModel filter);
    }
}
