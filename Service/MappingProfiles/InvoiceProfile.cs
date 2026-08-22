using AutoMapper;
using Domain.Models;
using Shared.DTOS.InvoiceDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.MappingProfiles
{
    public class InvoiceProfile : Profile
    {
        public InvoiceProfile()
        {
            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(dest => dest.ItemName, opt => opt.MapFrom(src => src.Product.Name));

            CreateMap<Order, OrderBasicDto>()
            .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.OrderTotal, opt => opt.MapFrom(src =>
                src.OrderItems.Sum(item => item.TotalPrice)
            ))
            .ForMember(dest => dest.OrderStatus, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.TableId, opt => opt.MapFrom(src => src.Invoice.TableId));

            CreateMap<Invoice, InvoiceDetailsDto>()
                .ForMember(dest => dest.InvoiceId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.WaiterName, opt => opt.MapFrom(src => src.User.FullName))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src =>
                    src.Orders.Any() ? src.Orders.Min(o => o.CreatedAt) : default(DateTime)
                ))
                .ForMember(dest => dest.Orders, opt => opt.MapFrom(src => src.Orders))
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src =>
                    src.Orders.Sum(o => o.OrderItems.Sum(item => item.TotalPrice))
                ))
                .ForMember(dest => dest.InvoiceStatus, opt => opt.MapFrom(src => src.Status.ToString()));

            CreateMap<Invoice, InvoiceDto>()
            .ForMember(dest => dest.InvoiceId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.WaiterName, opt => opt.MapFrom(src => src.User.FullName))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src =>
                src.Orders.Any() ? src.Orders.Min(o => o.CreatedAt) : default(DateTime)
            ))
            .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src =>
                src.Orders.Sum(o => o.OrderItems.Sum(item => item.TotalPrice))
            ))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));


            CreateMap<OrderStatusLog, InvoiceStatusLogDto>()
           .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.FullName)) 
           .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString())); // تحويل الـ Enum لنص
            
            CreateMap<Invoice, InvoiceDetailsWithStatusLog>()
                .ForMember(dest => dest.InvoiceId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.WaiterName, opt => opt.MapFrom(src => src.User.FullName))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src =>
                     src.Orders.Any() ? src.Orders.Min(o => o.CreatedAt) : default
                 ))
                .ForMember(dest => dest.StatusLogs, opt => opt.MapFrom(src =>
        // AutoMapper هيستخدم المابينج اللي فوق تلقائياً لكل عنصر
        src.Orders.SelectMany(o => o.StatusLogs).OrderBy(log => log.ChangedAt)
          ))
                .ForMember(dest => dest.InvoiceStatus, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src =>
                src.Orders.Sum(o => o.OrderItems.Sum(item => item.TotalPrice))
            ));


        }
    }
    
}
