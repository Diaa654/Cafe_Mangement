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
    public class ProductProfile: Profile
    {
        public ProductProfile() {

            CreateMap<AddProductDTO, Product>()
                .ForMember(des => des.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(des => des.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(des => des.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
                .ForMember(des=>des.Discount, opt => opt.MapFrom(src => src.Discount))
                .ForMember(des=>des.IsAvailable, opt => opt.MapFrom(src => src.IsAvailable));
        
        
        }
    }
}
