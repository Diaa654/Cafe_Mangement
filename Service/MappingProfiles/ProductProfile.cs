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


            CreateMap<Product, GetAllProductDTO>()
            .ForMember(dest => dest.ImageURL, opt => opt.MapFrom<URLResolver<Product, GetAllProductDTO>, string?>(src => src.ImageUrl))

            .ForMember(dest => dest.IsDiscount, opt => opt.MapFrom(src => src.Discount > 0))
            .ForMember(dest => dest.PriceAfterDiscount, opt => opt.MapFrom(src =>
                src.Discount > 0
                    ? (src.Price - (src.Price * (src.Discount / 100m)))
                    : src.Price))
                        .ForMember(dest => dest.IsAvailable, opt => opt.MapFrom(src => src.IsAvailable))
                        .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));

            CreateMap<Product, GetTopProductDTO>()
              .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom<URLResolver<Product, GetTopProductDTO>, string?>(src => src.ImageUrl))
              .ForMember(dest => dest.NumberOfSales, opt => opt.MapFrom(src => src.NumberOfSales))
               .ForMember(des => des.Name, opt => opt.MapFrom(src => src.Name));

        }
    }
}
