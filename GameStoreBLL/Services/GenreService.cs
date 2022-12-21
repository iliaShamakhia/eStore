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
    public class GenreService : IGenreService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        private readonly GenreModelValidator _validator;

        public GenreService(IUnitOfWork unitOfWork, IMapper mapper, GenreModelValidator validator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task AddAsync(GenreModel model)
        {
            if (model == null || !_validator.Validate(model).IsValid)
            {
                throw new GameStoreException("Invalid model");
            }

            var genre = await _unitOfWork.GenreRepository.GetByIdAsync(model.Id);

            if (genre != null)
            {
                throw new GameStoreException("Genre already exists");
            }
            else
            {
                await _unitOfWork.GenreRepository.AddAsync(_mapper.Map<Genre>(model));
                await _unitOfWork.SaveAsync();
            }
        }

        public async Task DeleteAsync(int modelId)
        {
            await _unitOfWork.GenreRepository.DeleteByIdAsync(modelId);
            await _unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<GenreModel>> GetAllAsync()
        {
            var genres = await _unitOfWork.GenreRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<Genre>, IEnumerable<GenreModel>>(genres);
        }

        public async Task<GenreModel> GetByIdAsync(int id)
        {
            var genre = await _unitOfWork.GenreRepository.GetByIdAsync(id);
            if (genre == null)
            {
                return null;
            }
            return _mapper.Map<GenreModel>(genre);
        }

        public async Task UpdateAsync(GenreModel model)
        {
            if (model == null || !_validator.Validate(model).IsValid)
            {
                throw new GameStoreException("Invalid model");
            }
            else
            {
                _unitOfWork.GenreRepository.Update(_mapper.Map<Genre>(model));
                await _unitOfWork.SaveAsync();
            }
        }
    }
}
