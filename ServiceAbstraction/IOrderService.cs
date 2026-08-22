using Shared.CommonResult;
using Shared.DTOS.InvoiceDto;
using Shared.DTOS.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbstraction
{
    public interface IOrderService
    {
        Task<Result<string>> CreateOrderAsync(int invoiceId, int waiterId, CreateOrderDto orderDto);
        Task<Result<string>> UpdateOrderAsync(int orderId, int waiterId, UpdateOrderDto orderDto);
        Task<Result<string>> ChangeOrderStatusAsync(int orderId, int userId, OrderStatus newStatus);
        Task<Result<IEnumerable<OrderBasicDto>>> GetActiveOrdersForBaristaAsync(int UserId);
        Task<Result<IEnumerable<OrderBasicDto>>> GetActiveOrdersForWaiterAsync(int USerId);
        Task<Result<OrderBasicDto>> GetOrderById(int orderId,int UserId);
        Task<Result<string>> CancelOrderById(int orderId, int userId);
    }
}
