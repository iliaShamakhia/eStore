using AutoMapper;
using GameStoreBLL.Models;
using GameStoreDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStoreBLL
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Game, GameModel>().ReverseMap();
            CreateMap<Comment, CommentModel>().ReverseMap();
            CreateMap<Genre, GenreModel>()
                .ForMember(gm => gm.GamesIds, g => g.MapFrom(x => x.Games.Select(g => g.Id)))
                .ReverseMap();
            CreateMap<Order, OrderModel>().ReverseMap();
            CreateMap<OrderDetail, OrderDetailModel>().ReverseMap();
            CreateMap<User, UserModel>().ReverseMap();
        }
    }
}
