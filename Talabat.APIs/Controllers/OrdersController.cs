using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.Core.Entities.Order_Aggregation;
using Talabat.Core.Services;

namespace Talabat.APIs.Controllers
{
    [Authorize]
    public class OrdersController : ApiBaseController
    {
        private readonly IOrderService orderService;
        private readonly IMapper mapper;

        public OrdersController(IOrderService orderService,IMapper mapper)
        {
            this.orderService = orderService;
            this.mapper = mapper;
        }
        [ProducesResponseType(typeof(Order),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(OrderDto orderDto)
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var address = mapper.Map<AddressDto, Address>(orderDto.ShippingAddress);
            var Order= await orderService.CreateOrderAsync(buyerEmail,orderDto.basketId,orderDto.DeliveryMethodId,address);
            if (Order is null) return BadRequest(new ApiErrorResponse(400));
            return Ok(Order);
        }
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Order>>> GetOrderForsUser()
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var Orders= await orderService.GetOrderForUserAsync(buyerEmail);
            return Ok(Orders);
        }
        [ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrderForUser(int id)
        {
            var buyerEmail=User.FindFirstValue(ClaimTypes.Email);
            var Order= await orderService.CreateOrderByIdForUserAsync(id,buyerEmail);
            if (Order is null) return NotFound(new ApiErrorResponse(404));
            return Ok(Order) ;
        }
        [HttpGet("deliverymethods")]
        public async Task<ActionResult<IReadOnlyList< DeliveryMethod>>> GetDeliveryMethods()
        {
            var deliverymethods=await orderService.GetDeliveryMethodsAsync();
            return Ok(deliverymethods);
        }
    }
}
