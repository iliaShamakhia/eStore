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
    public class CommentRepository : ICommentRepository
    {
        private readonly GameStoreDbContext _context;

        public CommentRepository(GameStoreDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Comment entity)
        {
            await _context.Comments.AddAsync(entity);
        }

        public void Delete(Comment entity)
        {
            _context.Comments.Remove(entity);
        }

        public async Task DeleteByIdAsync(int id)
        {
            var comment = await _context.Comments.Include(c => c.ChildComments)
                .FirstOrDefaultAsync(c => c.Id == id);
            

            if (comment == null) return;

            if(comment.ChildComments.Count > 0)
            {
                foreach(var child in comment.ChildComments)
                {
                    _context.Comments.Remove(child);
                }
            }

            _context.Comments.Remove(comment);
        }

        public async Task<IEnumerable<Comment>> GetAllAsync()
        {
            return await _context.Comments
                .Include(c => c.ChildComments)
                .ToListAsync();
        }

        public async Task<Comment> GetByIdAsync(int id)
        {
            var comments = await GetAllAsync();
            var comment = comments.FirstOrDefault(c => c.Id == id);

            if (comment == null) return null;

            return comment;
        }

        public void Update(Comment entity)
        {
            var childsToUpdate = _context.Comments.Where(c => c.ParentCommentId == entity.Id).ToList();
            entity.ChildComments = childsToUpdate;
            _context.Comments.Update(entity);
        }
    }
}
