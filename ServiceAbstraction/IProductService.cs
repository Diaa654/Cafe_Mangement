using Microsoft.AspNetCore.Http;
using Shared.CommonResult;
using Shared.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbstraction
{
    public interface IProductService
    {
        Task<Result> AddAsync(AddProductDTO dTO);
        Task<Result> DeleteAsync(int ProductId);
        Task<Result> UpdateAsync(int ProductId, UpdateProductDto dto);
        Task<Result> UpdateDiscountAsync(int productId, decimal newDiscount);
        Task<Result> ToggleAvailabilityAsync(int productId);
        Task<Result> UpdateImageProduct(IFormFile image);
        Task<Result<IEnumerable<GetAllProductDTO>>> GetProductsByCategoryAsync(int categoryId);
    }
}
