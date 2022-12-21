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
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        private readonly CommentModelValidator _validator;

        public CommentService(IUnitOfWork unitOfWork, IMapper mapper, CommentModelValidator validator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task AddAsync(CommentModel model)
        {
            if (model == null || !_validator.Validate(model).IsValid)
            {
                throw new GameStoreException("Invalid model");
            }
            else
            {
                await _unitOfWork.CommentRepository.AddAsync(_mapper.Map<Comment>(model));
                await _unitOfWork.SaveAsync();
            }
        }

        public async Task DeleteAsync(int modelId)
        {
            await _unitOfWork.CommentRepository.DeleteByIdAsync(modelId);
            await _unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<CommentModel>> GetAllAsync()
        {
            var comments = await _unitOfWork.CommentRepository.GetAllAsync();
            comments = comments.Where(c => c.ParentCommentId == null);
            return _mapper.Map<IEnumerable<Comment>, IEnumerable<CommentModel>>(comments);
        }

        public async Task<CommentModel> GetByIdAsync(int id)
        {
            var comment = await _unitOfWork.CommentRepository.GetByIdAsync(id);
            return _mapper.Map<CommentModel>(comment);
        }

        public async Task UpdateAsync(int id, CommentModel model)
        {
            if (model == null || !_validator.Validate(model).IsValid)
            {
                throw new GameStoreException("Invalid model");
            }
            else
            {
                var comment = await _unitOfWork.CommentRepository.GetByIdAsync(id);
                if(comment == null)
                {
                    throw new GameStoreException("comment not found");
                }
                _mapper.Map(model, comment);
                _unitOfWork.CommentRepository.Update(comment);
                await _unitOfWork.SaveAsync();
            }
        }
    }
}
