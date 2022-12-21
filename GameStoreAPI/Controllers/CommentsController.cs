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
    public class CommentsController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentsController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CommentModel>>> Get()
        {
            var comments = await _commentService.GetAllAsync();
            return Ok(comments);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<CommentModel>>> GetByGameId(int id)
        {
            var comments = await _commentService.GetAllAsync();
            comments = comments.Where(c => c.GameId == id);
            return Ok(comments);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Add([FromBody] CommentModel value)
        {
            try
            {
                await _commentService.AddAsync(value);
            }
            catch (GameStoreException ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(value);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] CommentModel value)
        {
            try
            {
                await _commentService.UpdateAsync(id, value);
            }
            catch (GameStoreException ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(value);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _commentService.DeleteAsync(id);
            return Ok();
        }
    }
}
