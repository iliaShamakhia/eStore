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
    public class OrderDetailService : IOrderDetailService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        public OrderDetailService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OrderDetailModel>> GetAllByOrderIdAsync(int id)
        {
            var orderDetails = await _unitOfWork.OrderDetailRepository.GetAllAsync();
            orderDetails = orderDetails.Where(od => od.OrderId == id);
            return _mapper.Map<IEnumerable<OrderDetail>, IEnumerable<OrderDetailModel>>(orderDetails);
        }

        public async Task IncreaseQuantity(int id)
        {
            var orderDetail = await _unitOfWork.OrderDetailRepository.GetByIdAsync(id);
            var game = await _unitOfWork.GameRepository.GetByIdAsync(orderDetail.GameId);
            orderDetail.Quantity++;
            orderDetail.Price += game.Price;
            _unitOfWork.OrderDetailRepository.Update(orderDetail);
            await _unitOfWork.SaveAsync();
        }

        public async Task DecreaseQuantity(int id)
        {
            var orderDetail = await _unitOfWork.OrderDetailRepository.GetByIdAsync(id);
            var game = await _unitOfWork.GameRepository.GetByIdAsync(orderDetail.GameId);
            if(orderDetail.Quantity > 0)
            {
                orderDetail.Quantity--;
                orderDetail.Price -= game.Price;
            }
            else
            {
                return;
            }
            _unitOfWork.OrderDetailRepository.Update(orderDetail);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(int modelId)
        {
            await _unitOfWork.OrderDetailRepository.DeleteByIdAsync(modelId);
            await _unitOfWork.SaveAsync();
        }

        public async Task AddAsync(OrderDetailModel model)
        {
            if(model == null)
            {
                throw new GameStoreException();
            }

            var orderDetail = await _unitOfWork.OrderDetailRepository.GetByIdAsync(model.Id);

            if (orderDetail != null)
            {
                orderDetail.Quantity++;
                orderDetail.Price += orderDetail.Game.Price;
                _unitOfWork.OrderDetailRepository.Update(orderDetail);
            }
            else
            {
                await _unitOfWork.OrderDetailRepository.AddAsync(_mapper.Map<OrderDetail>(model));
            }
            await _unitOfWork.SaveAsync();
        }
    }
}
