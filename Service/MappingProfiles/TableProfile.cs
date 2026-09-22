using AutoMapper;
using Domain.Models;
using Shared.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.MappingProfiles
{
    public class TableProfile:Profile
    {
        public TableProfile()
        {
            CreateMap<Table, TableDetailsDto>()
    .ForMember(dest => dest.TableId, opt => opt.MapFrom(src => src.Id))
    .ForMember(dest=>dest.InvoiceId,opt=>opt.MapFrom(src=>
        src.Invoices.Select(invoice => invoice.Id).FirstOrDefault()
    ))

    .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src =>
        src.Invoices
            .Where(i => i.Status == InvoiceStatus.Pending) 
            .Select(i => i.TotalAmount)
            .FirstOrDefault()
    ))

    
    .ForMember(dest => dest.OrderStatus, opt => opt.MapFrom(src =>
        src.Invoices
            .Where(i => i.Status == InvoiceStatus.Pending) 
            .SelectMany(i => i.Orders ?? new List<Order>())
            .OrderByDescending(o => o.Id)
            .Select(o => o.Status.ToString())
            .FirstOrDefault() ?? (src.IsAvailable ? "فارغ" : "غير متاح")//غير متاح يعنى مفيش طلبات على الفاتورة ده
    ));
        }
    }
}
