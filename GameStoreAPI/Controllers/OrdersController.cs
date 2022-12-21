using GameStoreBLL;
using GameStoreBLL.Interfaces;
using GameStoreBLL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<OrderModel>>> GetByUserId(string id)
        {
            var order = await _orderService.GetOrderByUserIdAsync(id);
            if(order == null)
            {
                return Ok(null);
            }
            return Ok(order);
        }

        [HttpPost]
        public async Task<ActionResult> Add([FromBody] OrderModel value)
        {
            try
            {
                await _orderService.AddAsync(value);
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
            await _orderService.DeleteAsync(id);
            return Ok();
        }
    }
}
