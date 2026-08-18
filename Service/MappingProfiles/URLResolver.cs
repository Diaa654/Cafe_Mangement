using AutoMapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.MappingProfiles
{
    using AutoMapper;
    using Microsoft.Extensions.Configuration;

    public class URLResolver<TSource, TDestination>(IConfiguration _configuration)
        : IMemberValueResolver<TSource, TDestination, string?, string?>
    {
        public string? Resolve(TSource source, TDestination destination, string? sourceMember, string? destMember, ResolutionContext context)
        {
            
            if (string.IsNullOrWhiteSpace(sourceMember))
                return string.Empty;
            var baseUrl = _configuration.GetSection("Urls")["BaseUrl"];

            
            return $"{baseUrl?.TrimEnd('/')}/{sourceMember.TrimStart('/')}";
        }
    }
}
