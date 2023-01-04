using System.Security.Claims;
using Business.Abstract;
using Entity.Concrete;
using Entity.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
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

        [HttpGet]
        public IActionResult GetAll()
        {
            var result = _orderService.GetAll();
            if (result.IsSuccess)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpGet("getbyuserid/{id}")]
        public IActionResult GetByUserId(string id)
        {
            var result = _orderService.GetByUserId(id);
            if (result.IsSuccess)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpPost("add")]
        public IActionResult AddOrder(Order order)
        {
            var result = _orderService.Add(order);
            if (result.IsSuccess)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpGet("getbyid/{id}")]
        public IActionResult GetById(int id)
        {
            var result = _orderService.GetById(id);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("getwithdetails/{id}")]
        public IActionResult GetWithDetails(int id)
        {
            var result = _orderService.GetOrderWithDetails(id);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("createorder")]
        public IActionResult CreateOrder(OrderPostDto order)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            order.UserId = userId;
            var result = _orderService.CreateOrder(order);
            if (result.IsSuccess)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpGet("getorders")]
        public IActionResult GetOrders()
        {
            var result = _orderService.GetOrders();
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("getordersuser")]
        public IActionResult GetOrdersUser()
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = _orderService.GetOrdersUser(userId);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
