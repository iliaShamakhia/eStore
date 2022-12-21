using GameStoreBLL;
using GameStoreBLL.Interfaces;
using GameStoreBLL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameStoreDAL.Data.Helpers;

namespace GameStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly IGameService _gameService;

        public GamesController(IGameService gameService)
        {
            _gameService = gameService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GameModel>>> Get([FromQuery] string title, string genre)
        {
            if (!string.IsNullOrEmpty(title) || !string.IsNullOrEmpty(genre))
            {
                var filter = new FilterSearchModel { Title = title, Genre = genre };
                var products = await _gameService.GetGamesByFilterAsync(filter);
                return Ok(products);
            }
            else
            {
                var products = await _gameService.GetAllAsync();
                return Ok(products);
            }

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GameModel>> GetById(int id)
        {
            var game = await _gameService.GetByIdAsync(id);
            if (game == null)
            {
                return NotFound();
            }
            return Ok(game);
        }

        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Manager)]
        [HttpPost]
        public async Task<ActionResult> Add([FromBody] GameModel value)
        {
            try
            {
                await _gameService.AddAsync(value);
            }
            catch (GameStoreException ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(value);
        }

        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Manager)]
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] GameModel value)
        {
            var game = await _gameService.GetByIdAsync(id);

            if(game == null)
            {
                return NotFound();
            }

            try
            {
                await _gameService.UpdateAsync(value);
            }
            catch (GameStoreException ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(value);
        }

        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Manager)]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _gameService.DeleteAsync(id);
            return Ok();
        }
    }
}
