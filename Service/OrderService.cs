using AutoMapper;
using Domain.Contracts;
using Domain.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Service.Specifications;
using ServiceAbstraction;
using Shared;
using Shared.CommonResult;
using Shared.DTOS.InvoiceDto;
using Shared.DTOS.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InvoiceStatus = Domain.Models.InvoiceStatus;
using OrderStatus = Shared.DTOS.InvoiceDto.OrderStatus;

namespace Service
{
    public class OrderService(IUnitOfWork _unitOfWork, IMapper _mapper,IFcmService _fcmService
        , UserManager<User> userManager,ILogger _logger) : IOrderService
    {
        public async Task<Result<string>> CreateOrderAsync(int invoiceId, int waiterId, CreateOrderDto orderDto)
        {
            var user = await _unitOfWork.GetRepository<User, int>().GetByIdAsync(waiterId);
            if (user == null)
                return Error.Failure("User Not Found", "الويتر غير موجود");

            if (!user.IsActive)
                return Error.Failure("User Inactive", "لا يمكنك إجراء أي نشاط في الشيفت الحالي");

            var invoiceRepo = _unitOfWork.GetRepository<Invoice, int>();
            var productRepo = _unitOfWork.GetRepository<Product, int>();

            var invoice = await invoiceRepo.GetByIdAsync(invoiceId);
            if (invoice == null || invoice.Status != InvoiceStatus.Pending)
                return Error.Failure("Invalid Invoice", "الفاتورة غير موجودة أو مغلقة.");

            var orderItems = new List<OrderItem>();

            
            foreach (var item in orderDto.Items)
            {
                var product = await productRepo.GetByIdAsync(item.ProductId);
                if (product == null)
                    return Error.Failure("Product Not Found", $"العنصر رقم {item.ProductId} غير موجود بالمنيو.");

               
                product.NumberOfSales += item.Quantity;

                orderItems.Add(new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = product.Price,
                    Discount = product.Discount,
                });
            }

            var order = new Order
            {
                InvoiceId = invoiceId,
                Status = Domain.Models.OrderStatus.Waiting,
                CreatedAt = DateTime.UtcNow,
                OrderItems = orderItems,
                StatusLogs = new List<OrderStatusLog>
        {
            new OrderStatusLog
            {
                UserId = waiterId,
                Status = Domain.Models.OrderStatus.Waiting,
                ChangedAt = DateTime.UtcNow
            }
        }
            };

            await _unitOfWork.GetRepository<Order, int>().AddAsync(order);

            
            int rowsAffected = await _unitOfWork.SaveChangesAsync();
            if (rowsAffected <= 0)
                return Error.Failure("Order Creation Failed", "فشل إنشاء الطلب.");

            await _fcmService.SendToTopicAsync("Barista", "طلب جديد", "تم إنشاء طلب جديد، يرجى التحقق من الطلبات الجديدة.");

            return Result<string>.Ok($"تم إنشاء الطلب بنجاح.  {order.InvoiceId}");
        }
        public async Task<Result<string>> UpdateOrderAsync(int orderId, int waiterId, UpdateOrderDto orderDto)
        {
            var user =await _unitOfWork.GetRepository<User, int>().GetByIdAsync(waiterId);
            if (user == null)
                return Error.Failure("User Not Found", "الويتر غير موجود");
            if (!user.IsActive)
                return Error.Failure("User Inactive", "لا يمكنك إجراء أي نشاط في الشيفت الحالي");
            var orderRepo = _unitOfWork.GetRepository<Order, int>();
            var productRepo = _unitOfWork.GetRepository<Product, int>();
            var spec = new OrderWithItemsSpecifications(orderId);
            var order = await orderRepo.GetByIdAsync(spec);

            if (order == null)
                return Error.Failure("Order Not Found", "الطلب غير موجود.");
            if (order.Status != (Domain.Models.OrderStatus)OrderStatus.Waiting)
                return Error.Failure("Cannot Update", "لا يمكن تعديل الطلب بعد بدء تحضيره أو تسليمه.");

            var incomingItemIds = orderDto.Items.Select(i => i.Id).ToList();
            var itemsToRemove = order.OrderItems.Where(i => !incomingItemIds.Contains(i.Id)).ToList();
            foreach (var item in itemsToRemove)
            {
                order.OrderItems.Remove(item);
            }

            foreach (var dtoItem in orderDto.Items)
            {
                if (dtoItem.Id > 0)
                {
                   
                    var existingItem = order.OrderItems.FirstOrDefault(i => i.Id == dtoItem.Id);
                    if (existingItem != null)
                    {
                        existingItem.Quantity = dtoItem.Quantity;
                    }
                }
                else
                {
                    
                    var product = await productRepo.GetByIdAsync(dtoItem.ProductId);
                    if (product != null)
                    {
                        order.OrderItems.Add(new OrderItem
                        {
                            ProductId = dtoItem.ProductId,
                            Quantity = dtoItem.Quantity,
                            UnitPrice = product.Price, 
                            Discount = product.Discount
                        });
                    }
                }
            }

            orderRepo.Update(order);
            int res=await _unitOfWork.SaveChangesAsync();
            if (res == 0)
                return Error.Failure("Order Update Failed", "فشل تحديث الطلب.");
            //await _fcmService.SendToTopicAsync("Barista","تم تعديل الطلب",$"تم تعديل الطلب، يرجى التحقق من الطلبات الجديدة.  {order.Id}");
            return Result<string>.Ok("تم تحديث الطلب بنجاح.");
        }
        public async Task<Result<string>> ChangeOrderStatusAsync(int orderId, int userId, OrderStatus newStatus)
        {
            var user =await _unitOfWork.GetRepository<User, int>().GetByIdAsync(userId);
            if (user == null)
                return Error.Failure("User Not Found", "المستخدم غير موجود.");
            if (!user.IsActive)
                return Error.Failure("User Inactive", "لا يمكنك إجراء أي نشاط في الشيفت الحالي");
            var orderRepo = _unitOfWork.GetRepository<Order, int>();
            var spec = new OrderWithStatusLogsSpecifications(orderId);
            var order = await orderRepo.GetByIdAsync(spec);

            if (order == null)
                return Error.Failure("Order Not Found", "الطلب غير موجود.");

            if (order.Status == (Domain.Models.OrderStatus)newStatus)
                return Error.Failure("Invalid Status", "الطلب موجود في هذه الحالة بالفعل.");
            
            var userRole = await userManager.GetRolesAsync(user);
            if(newStatus == OrderStatus.Preparing && !userRole.Contains(AppRoles.Barista.ToString()))
                return Error.Failure("Unauthorized", "ليس لديك صلاحية لتغيير حالة الطلب إلى تحضير.");
            if(newStatus == OrderStatus.Ready && !userRole.Contains(AppRoles.Barista.ToString()))
                return Error.Failure("Unauthorized", "ليس لديك صلاحية لتغيير حالة الطلب إلى جاهز.");
            if(newStatus == OrderStatus.Delivered && !userRole.Contains(AppRoles.Waiter.ToString()))
                return Error.Failure("Unauthorized", "ليس لديك صلاحية لتغيير حالة الطلب إلى تم التسليم.");
            order.Status = (Domain.Models.OrderStatus)newStatus;
            order.StatusLogs.Add(new OrderStatusLog
            {
                UserId = userId, 
                Status = (Domain.Models.OrderStatus)newStatus,
                ChangedAt = DateTime.UtcNow
            });

            orderRepo.Update(order);
            int res=await _unitOfWork.SaveChangesAsync();
            if (res == 0)
                return Error.Failure("Order Status Change Failed", "فشل تغيير حالة الطلب.");
            if(newStatus==OrderStatus.Ready)
                await _fcmService.SendToTopicAsync("Waiters","طلب جاهز للتسليم",$"تم تغيير حالة الطلب، يرجى التحقق من الطلبات الجديدة.  {order.Invoice.TableId}");

            return Result<string>.Ok("تم تغيير حالة الطلب بنجاح.");
        }

        public async Task<Result<IEnumerable<OrderBasicDto>>> GetActiveOrdersForBaristaAsync(int UserId)
        {
            var user = await _unitOfWork.GetRepository<User, int>().GetByIdAsync(UserId);
            if (user == null)
                return Error.Failure("User Not Found", "المستخدم غير موجود.");
            if (!user.IsActive)
                return Error.Failure("User Inactive", "لا يمكنك إجراء أي نشاط في الشيفت الحالي");
            var spec = new BaristaOrdersSpecifications();
            var orders = await _unitOfWork.GetRepository<Order, int>().GetAllAsync(spec);
            var ordersDto = _mapper.Map<IEnumerable<OrderBasicDto>>(orders);
            return Result<IEnumerable<OrderBasicDto>>.Ok(ordersDto);
        }

        public async Task<Result<IEnumerable<OrderBasicDto>>> GetActiveOrdersForWaiterAsync(int UserId)
        {
            var user = await _unitOfWork.GetRepository<User, int>().GetByIdAsync(UserId);
            if (user == null)
                return Error.Failure("User Not Found", "المستخدم غير موجود.");
            if (!user.IsActive)
                return Error.Failure("User Inactive", "لا يمكنك إجراء أي نشاط في الشيفت الحالي");
            var spec = new WaiterOrderSpecifications();
            var orders = await _unitOfWork.GetRepository<Order, int>().GetAllAsync(spec);
            var ordersDto = _mapper.Map<IEnumerable<OrderBasicDto>>(orders);

            return Result<IEnumerable<OrderBasicDto>>.Ok(ordersDto);
        }
        public async Task<Result<OrderBasicDto>> GetOrderById(int orderId,int UserId)
        {
            var user = await _unitOfWork.GetRepository<User, int>().GetByIdAsync(UserId);
            if (user == null)
                return Error.Failure("User Not Found", "المستخدم غير موجود.");
            if (!user.IsActive)
                return Error.Failure("User Inactive", "لا يمكنك إجراء أي نشاط في الشيفت الحالي");
            var spec = new OrderWithItemsSpecifications(orderId);
            var order = await _unitOfWork.GetRepository<Order, int>().GetByIdAsync(spec);
            if (order == null)
                return Error.Failure("Order Not Found", "الطلب غير موجود.");
            var orderDto = _mapper.Map<OrderBasicDto>(order);
            return Result<OrderBasicDto>.Ok(orderDto);
        }

        public async Task<Result<string>> CancelOrderById(int orderId, int userId)
        {
            var user = await _unitOfWork.GetRepository<User, int>().GetByIdAsync(userId);
            if (user == null)
                return Error.Failure("User Not Found", "المستخدم غير موجود.");
            if (!user.IsActive)
                return Error.Failure("User Inactive", "لا يمكنك إجراء أي نشاط في الشيفت الحالي");

            var orderRepo = _unitOfWork.GetRepository<Order, int>();
            var order=await orderRepo.GetByIdAsync(orderId);
            if (order == null)
                return Error.NotFound("Order Not Found", "الطلب غير موجود");
            if (order.Status != (Domain.Models.OrderStatus)OrderStatus.Waiting)
                return Error.Failure("Order Can Not be Canceled", "لا يمكنك تعديل الطلب فى هذه الحالة");
            order.Status = Domain.Models.OrderStatus.Canceled;
            await _unitOfWork.SaveChangesAsync();
            return Result<string>.Ok("تم الغاء الطلب بنجاح");

        }
    }
}
