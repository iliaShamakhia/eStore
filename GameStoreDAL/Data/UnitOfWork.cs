using GameStoreDAL.Interfaces;
using GameStoreDAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStoreDAL.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GameStoreDbContext _context;

        private IGameRepository _gameRepository;
        private ICommentRepository _commentRepository;
        private IOrderRepository _orderRepository;
        private IOrderDetailRepository _orderDetailRepository;
        private IGenreRepository _genreRepository;

        public UnitOfWork(GameStoreDbContext context)
        {
            _context = context;
        }

        public IGameRepository GameRepository
        {
            get
            {
                if(_gameRepository == null)
                {
                    _gameRepository = new GameRepository(_context);
                }

                return _gameRepository;
            }       
        }

        public ICommentRepository CommentRepository
        {
            get
            {
                if (_commentRepository == null)
                {
                    _commentRepository = new CommentRepository(_context);
                }

                return _commentRepository;
            }
        }

        public IOrderRepository OrderRepository
        {
            get
            {
                if (_orderRepository == null)
                {
                    _orderRepository = new OrderRepository(_context);
                }

                return _orderRepository;
            }
        }

        public IOrderDetailRepository OrderDetailRepository
        {
            get
            {
                if (_orderDetailRepository == null)
                {
                    _orderDetailRepository = new OrderDetailRepository(_context);
                }

                return _orderDetailRepository;
            }
        }

        public IGenreRepository GenreRepository
        {
            get
            {
                if (_genreRepository == null)
                {
                    _genreRepository = new GenreRepository(_context);
                }

                return _genreRepository;
            }
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
