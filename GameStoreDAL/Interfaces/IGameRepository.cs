﻿using GameStoreDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStoreDAL.Interfaces
{
    public interface IGameRepository : IRepository<Game>
    {
        Task<Game> GetByTitleAsync(string title);
    }
}
