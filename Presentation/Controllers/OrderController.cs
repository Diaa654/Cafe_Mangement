using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceAbstraction;
using Shared;
using Shared.DTOS.InvoiceDto;
using Shared.DTOS.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    public class OrderController(IServiceManger serviceManger): ApiBaseController
    {
        [HttpPost("AddOrderToInvoice/{invoiceId}")]
        [Authorize(Roles = nameof(AppRoles.Waiter) + "," + nameof(AppRoles.Barista))]
        public async Task<ActionResult<string>> AddOrderToInvoice(int invoiceId, [FromBody]CreateOrderDto orderDto)
        {
            var userId = GetUserId();
            var result = await serviceManger.OrderService.CreateOrderAsync(invoiceId, userId, orderDto);
            return HandleResult(result);
        }
        [HttpPost("UpdateOrder/{orderId}")]
        [Authorize(Roles = nameof(AppRoles.Waiter) + "," + nameof(AppRoles.Barista))]
        public async Task<ActionResult<string>> UpdateOrder(int orderId, [FromBody] UpdateOrderDto orderDto)
        {
            var userId = GetUserId();
            var result = await serviceManger.OrderService.UpdateOrderAsync(orderId, userId, orderDto);
            return HandleResult(result);
        }
        [HttpPost("ChangeOrderStatus/{orderId}")]
        [Authorize(Roles = nameof(AppRoles.Waiter) + "," + nameof(AppRoles.Barista))]
        public async Task<ActionResult<string>> ChangeOrderStatus(int orderId, [FromQuery] OrderStatus newStatus)
        {
            var userId = GetUserId();
            var result = await serviceManger.OrderService.ChangeOrderStatusAsync(orderId, userId, newStatus);
            return HandleResult(result);
        }
        [HttpGet("GetActiveOrdersForWaiter")]
        [Authorize(Roles = nameof(AppRoles.Waiter))]
        public async Task<ActionResult<IEnumerable<OrderBasicDto>>> GetActiveOrdersForWaiter()
        {
            var userId = GetUserId();
            var result = await serviceManger.OrderService.GetActiveOrdersForWaiterAsync(userId);
            return HandleResult(result);
        }
        [HttpGet("GetOrderById/{orderId}")]
        [Authorize(Roles = nameof(AppRoles.Waiter) + "," + nameof(AppRoles.Barista))]
        public async Task<ActionResult<OrderBasicDto>> GetOrderById(int orderId)
        {
            var userId = GetUserId();
            var result = await serviceManger.OrderService.GetOrderById(orderId,userId);
            return HandleResult(result);
        }
        [HttpGet("GetActiveOrdersForBarista")]
        [Authorize(Roles = nameof(AppRoles.Barista))]
        public async Task<ActionResult<IEnumerable<OrderBasicDto>>> GetActiveOrdersForBarista()
        {
            var userId = GetUserId();
            var result = await serviceManger.OrderService.GetActiveOrdersForBaristaAsync(userId);
            return HandleResult(result);
        }
        [HttpPost("CancelOrderWithId/{OrderId}")]
        [Authorize(Roles = nameof(AppRoles.Waiter))]
        public async Task<ActionResult<string>> CancelOrder(int OrderId)
        {
            int UserId=GetUserId();
            var result=await serviceManger.OrderService.CancelOrderById(OrderId,UserId);
            return HandleResult(result);
        }
    }
}
