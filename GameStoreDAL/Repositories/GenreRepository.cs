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
    public class GenreRepository : IGenreRepository
    {
        private readonly GameStoreDbContext _context;

        public GenreRepository(GameStoreDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Genre entity)
        {
            await _context.Genres.AddAsync(entity);
        }

        public void Delete(Genre entity)
        {
            _context.Genres.Remove(entity);
        }

        public async Task DeleteByIdAsync(int id)
        {
            var genre = await _context.Genres.FindAsync(id);

            if (genre == null) return;

            _context.Genres.Remove(genre);
        }

        public async Task<IEnumerable<Genre>> GetAllAsync()
        {
            return await _context.Genres
                .Include(g => g.Games)
                .ToListAsync(); 
        }

        public async Task<Genre> GetByIdAsync(int id)
        {
            var genres = await GetAllAsync();

            var genre = genres.FirstOrDefault(g => g.Id == id);

            if (genre == null) return null;

            return genre;
        }

        public void Update(Genre entity)
        {
            _context.Genres.Update(entity);
        }
    }
}
