using AutoMapper;
using GameStoreBLL.Interfaces;
using GameStoreBLL.Models;
using GameStoreBLL.Validators;
using GameStoreDAL.Entities;
using GameStoreDAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStoreBLL.Services
{
    public class GameService : IGameService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        private readonly GameModelValidator _validator;

        public GameService(IUnitOfWork unitOfWork, IMapper mapper, GameModelValidator validator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task AddAsync(GameModel model)
        {
            if (model == null || !_validator.Validate(model).IsValid)
            {
                throw new GameStoreException("Invalid model");
            }

            var game = await _unitOfWork.GameRepository.GetByTitleAsync(model.Title);

            if (game != null)
            {
                throw new GameStoreException("game already exists");
            }
            else
            {
                await _unitOfWork.GameRepository.AddAsync(_mapper.Map<Game>(model));
                await _unitOfWork.SaveAsync();
            }
        }

        public async Task DeleteAsync(int modelId)
        {
            await _unitOfWork.GameRepository.DeleteByIdAsync(modelId);
            await _unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<GameModel>> GetAllAsync()
        {
            var games = await _unitOfWork.GameRepository.GetAllAsync();
            foreach(var game in games)
            {
                game.Comments = game.Comments.Where(c => c.ParentComment == null).ToList();
            }
            return _mapper.Map<IEnumerable<Game>, IEnumerable<GameModel>>(games);
        }

        public async Task<GameModel> GetByIdAsync(int id)
        {
            var game = await _unitOfWork.GameRepository.GetByIdAsync(id);
            if (game == null)
            {
                return null;
            }
            return _mapper.Map<GameModel>(game);
        }

        public async Task<IEnumerable<GameModel>> GetGamesByFilterAsync(FilterSearchModel filter)
        {
            var games = await _unitOfWork.GameRepository.GetAllAsync();

            if (filter.Title != null)
            {
                games = games.Where(p => p.Title.ToUpper().Contains(filter.Title.ToUpper()));
            }
            if (filter.Genre != null)
            {
                games = games.Where(f => f.Genres.Select(g => g.Name.ToUpper()).Contains(filter.Genre.ToUpper()));
            }

            return _mapper.Map<IEnumerable<Game>, IEnumerable<GameModel>>(games);

        }

        public async Task UpdateAsync(GameModel model)
        {
            if (model == null || !_validator.Validate(model).IsValid)
            {
                throw new GameStoreException("Invalid model");
            }
            else
            {
                _unitOfWork.GameRepository.Update(_mapper.Map<Game>(model));
                await _unitOfWork.SaveAsync();
            }
        }
    }
}
