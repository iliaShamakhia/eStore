using GameStoreBLL;
using GameStoreBLL.Interfaces;
using GameStoreBLL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly IGenreService _genreService;

        public GenresController(IGenreService genreService)
        {
            _genreService = genreService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GenreModel>>> Get()
        {
            var genres = await _genreService.GetAllAsync();
            return Ok(genres);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<GenreModel>>> GetByGameId(int id)
        {
            var genres = await _genreService.GetAllAsync();
            genres = genres.Where(c => c.GamesIds.Any(gId => gId == id));
            return Ok(genres);
        }

        [HttpPost]
        public async Task<ActionResult> Add([FromBody] GenreModel value)
        {
            try
            {
                await _genreService.AddAsync(value);
            }
            catch (GameStoreException ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(value);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] GenreModel value)
        {
            var genre = await _genreService.GetByIdAsync(id);

            if (genre == null)
            {
                return NotFound();
            }

            try
            {
                await _genreService.UpdateAsync(value);
            }
            catch (GameStoreException ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(value);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _genreService.DeleteAsync(id);
            return Ok();
        }
    }
}
