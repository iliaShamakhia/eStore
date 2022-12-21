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
    public class OrderDetailsController : ControllerBase
    {
        private readonly IOrderDetailService _orderDetailService;

        public OrderDetailsController(IOrderDetailService orderDetailService)
        {
            _orderDetailService = orderDetailService;
        }

        [HttpPost]
        public async Task<ActionResult> Add([FromBody] OrderDetailModel value)
        {
            try
            {
                await _orderDetailService.AddAsync(value);
            }
            catch (GameStoreException ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(value);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<OrderDetailModel>>> GetByOrderId(int id)
        {
            var orderDetails = await _orderDetailService.GetAllByOrderIdAsync(id);
            if (orderDetails == null)
            {
                return NotFound();
            }
            return Ok(orderDetails);
        }

        [HttpPut("{id}/increase")]
        public async Task<ActionResult> IncreaseQuantity(int id)
        {
            await _orderDetailService.IncreaseQuantity(id);
            return Ok();
        }

        [HttpPut("{id}/decrease")]
        public async Task<ActionResult> DecreaseQuantity(int id)
        {
            await _orderDetailService.DecreaseQuantity(id);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<OrderDetailModel>> DeleteById(int id)
        {
            await _orderDetailService.DeleteAsync(id);
            return Ok();
        }
    }
}
