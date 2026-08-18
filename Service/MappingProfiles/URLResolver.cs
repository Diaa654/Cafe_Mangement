using AutoMapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.MappingProfiles
{
    public class URLResolver<TSourse, TDestination>(IConfiguration _configuration) : IMemberValueResolver<TSourse, TDestination, string?, string?>
    {
        public string Resolve(TSourse source, TDestination destination, string SrcMember, string destMember, ResolutionContext context)
        {
            if (string.IsNullOrWhiteSpace(SrcMember))
                return string.Empty;

            var baseUrl = _configuration.GetSection("Urls")["BaseUrl"];
            return $"{baseUrl}{SrcMember}";
        }
    }
}
