using AutoMapper;
using Domain.Contracts;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ServiceAbstraction;
using Shared.CommonResult;
using Shared.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class ProductService(IUnitOfWork _unitOfWork, IFileService _fileService,IMapper _mapper,ILogger _logger) : IProductService
    {
        public async Task<Result> AddAsync(AddProductDTO dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Name) || dto.Price <= 0 || dto.CategoryId <=0 || dto.Image == null)
                return Error.Validation("بيانات_غير_صالحة", "تأكد من إدخال اسم المنتج، السعر، القسم، وإرفاق صورة صالحة.");
            string? uploadedFilePath = null;

            async Task RevertChangesAsync()
            {
                if (!string.IsNullOrEmpty(uploadedFilePath))
                {
                    await _fileService.DeleteAsync(uploadedFilePath); 
                }
            }
            try
            {
                var uploadResult = await _fileService.SaveFileAsync(dto.Image, "ProductImage");

                if (!uploadResult.IsSuccess)
                    return Error.Failure("حدث خطأ اثناء حفظ الصوره ", "حاول ترفع صوره اخرى ");

                uploadedFilePath = uploadResult.Value;

                var product = _mapper.Map<Product>(dto);
                product.ImageUrl = uploadedFilePath;

                await _unitOfWork.GetRepository<Product, int>().AddAsync(product);
                await _unitOfWork.SaveChangesAsync(); 

                return Result.Ok();
            }
            catch (Exception ex)
            {
                await RevertChangesAsync();
                _logger.LogError(ex, "Error while adding a new Product with Image...");
                return Error.Failure("خطأ_غير_متوقع", "حدث خطأ غير متوقع أثناء إضافة المنتج.");
            }
        }

        public Task<Result> DeleteAsync(int ProductId)
        {
            throw new NotImplementedException();
        }

        public Task<Result<IEnumerable<GetAllProductDTO>>> GetProductsByCategoryAsync(int categoryId)
        {
            throw new NotImplementedException();
        }

        public Task<Result> ToggleAvailabilityAsync(int productId)
        {
            throw new NotImplementedException();
        }

        public Task<Result> UpdateAsync(int ProductId, UpdateProductDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<Result> UpdateDiscountAsync(int productId, decimal newDiscount)
        {
            throw new NotImplementedException();
        }

        public Task<Result> UpdateImageProduct(IFormFile image)
        {
            throw new NotImplementedException();
        }
    }
}
