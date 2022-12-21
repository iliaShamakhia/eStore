using AutoMapper;
using GameStoreBLL.Interfaces;
using GameStoreBLL.Models;
using GameStoreDAL.Entities;
using GameStoreDAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStoreBLL.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        public OrderService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task AddAsync(OrderModel model)
        {
            if (model == null)
            {
                throw new GameStoreException("Invalid model");
            }

            var order = await _unitOfWork.OrderRepository.GetByUserIdAsync(model.UserId);

            if (order != null)
            {
                throw new GameStoreException("order already exists");
            }
            else
            {
                await _unitOfWork.OrderRepository.AddAsync(_mapper.Map<Order>(model));
                await _unitOfWork.SaveAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.OrderRepository.DeleteByIdAsync(id);
            await _unitOfWork.SaveAsync();
        }

        public async Task<OrderModel> GetOrderByUserIdAsync(string userId)
        {
            var order = await _unitOfWork.OrderRepository.GetByUserIdAsync(userId);

            return _mapper.Map<OrderModel>(order);
        }
    }
}
