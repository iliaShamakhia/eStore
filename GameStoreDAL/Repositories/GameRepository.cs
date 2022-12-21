using GameStoreDAL.Data;
using GameStoreDAL.Entities;
using GameStoreDAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStoreDAL.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly GameStoreDbContext _context;

        public GameRepository(GameStoreDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Game entity)
        {
            await _context.Games.AddAsync(entity);
        }

        public void Delete(Game entity)
        {
            _context.Games.Remove(entity);
        }

        public async Task DeleteByIdAsync(int id)
        {
            var game = await _context.Games.FindAsync(id);

            if (game == null) return;

            _context.Games.Remove(game);
        }

        public async Task<IEnumerable<Game>> GetAllAsync()
        {
            return await _context.Games
                .Include(g => g.Genres)
                .Include(g => g.Comments)
                .ToListAsync();
        }

        public async Task<Game> GetByIdAsync(int id)
        {
            var games = await GetAllAsync();

            var game = games.FirstOrDefault(g => g.Id == id);

            if (game == null) return null;

            return game;
        }

        public async Task<Game> GetByTitleAsync(string title)
        {
            var games = await GetAllAsync();

            var game = games.FirstOrDefault(g => g.Title == title);

            if (game == null) return null;

            return game;
        }

        public void Update(Game entity)
        {
            var gameToUpdate = _context.Games.FirstOrDefault(g => g.Id == entity.Id);
            var entityGenresIds = entity.Genres.Select(g => g.Id).ToList();
            var genresToUpdate = _context.Genres.Where(g => entityGenresIds.Contains(g.Id)).ToList();

            var entityCommentsIds = entity.Comments.Select(c => c.Id).ToList();
            var commentsToUpdate = _context.Comments.Where(c => entityCommentsIds.Contains(c.Id)).ToList();

            if (gameToUpdate == null) return;

            gameToUpdate.Title = entity.Title;
            gameToUpdate.Description = entity.Description;
            gameToUpdate.ImageUrl = entity.ImageUrl;
            gameToUpdate.Price = entity.Price;
            gameToUpdate.Genres = genresToUpdate;
            gameToUpdate.Comments = commentsToUpdate;

            _context.Games.Update(gameToUpdate);
        }
    }
}
