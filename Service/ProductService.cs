using AutoMapper;
using Domain.Contracts;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Service.Specifications;
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
    public class ProductService(IUnitOfWork _unitOfWork, IFileService _fileService,IMapper _mapper,UserManager<User> _userManager) : IProductService
    {
        public async Task<Result> AddAsync(AddProductDTO dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Name) || dto.Price <= 0 || dto.CategoryId <=0 || dto.Image == null)
                return Error.Validation("بيانات_غير_صالحة", " تأكد من إدخال اسم المنتج، السعر، وإرفاق صورة صالحةوتحديد الفئه التي ينتمي اليها المنتج");
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
               // _logger.LogError(ex, "Error while adding a new Product with Image...");
                return Error.Failure("خطأ_غير_متوقع", "حدث خطأ غير متوقع أثناء إضافة المنتج.");
            }
        }

        public async Task<Result> DeleteAsync(int ProductId)
        {

            var repository = _unitOfWork.GetRepository<Product, int>();


            var product = await repository.GetByIdAsync(ProductId);
            if (product == null)
                return Error.NotFound("غير_موجود", "المنتج المطلوب حذفه غير موجود.");

            repository.Remove(product);
            await _unitOfWork.SaveChangesAsync();
            if (!string.IsNullOrEmpty(product.ImageUrl))
            {
                await _fileService.DeleteAsync(product.ImageUrl);
            }

            return Result.Ok();
        }

        public async Task<Result<IEnumerable<GetAllProductDTO>>> GetProductsByCategoryAsync(int userId, int categoryId)
        {
                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
                 if (user == null)
                    return Error.NotFound("غير_موجود", "المستخدم المطلوب غير موجود في النظام.");
                if(!user.IsActive)
                    return Error.Forbidden("غير_مفعل", "المستخدم غير مفعل، يرجى التواصل مع الإدارة لتفعيل الحساب.");
                var repository = _unitOfWork.GetRepository<Product, int>();
                var sp=new ProductSpecification(categoryId);
                var allProducts = await repository.GetAllAsync(sp);
                var dtos = _mapper.Map<IEnumerable<GetAllProductDTO>>(allProducts);

                return Result<IEnumerable<GetAllProductDTO>>.Ok(dtos);
           
           
        }

        public async Task<Result> UpdateAvailabilityAsync(int productId, bool isAvailable)
        {
           
                var repository = _unitOfWork.GetRepository<Product, int>();

                var product = await repository.GetByIdAsync(productId);
                if (product == null)
                    return Error.NotFound("غير_موجود", "المنتج المطلوب غير موجود في النظام.");
                product.IsAvailable = isAvailable;

                repository.Update(product);
                await _unitOfWork.SaveChangesAsync();

                return Result.Ok();
            
        }

        public async Task<Result> UpdateAsync(int ProductId, UpdateProductDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Name) || dto.Price <= 0)
                return Error.Validation("بيانات_غير_صالحة", "تأكد من إدخال اسم المنتج بشكل صحيح وأن السعر أكبر من صفر.");

           
                var repository = _unitOfWork.GetRepository<Product, int>();
              
                var product = await repository.GetByIdAsync(ProductId);

                if (product == null)
                    return Error.NotFound("غير_موجود", "المنتج المطلوب تعديله غير موجود في النظام.");
                
                product.Name = dto.Name;
                product.Price = dto.Price;

                
                repository.Update(product);
                await _unitOfWork.SaveChangesAsync();

                return Result.Ok();
           
        }

        public async Task<Result> UpdateDiscountAsync(int productId, decimal newDiscount)
        {
            if (newDiscount < 0 || newDiscount > 100)
                return Error.Validation("قيمة_مرفوضة", "نسبة الخصم يجب أن تكون بين 0 و 100.");

           
                var repository = _unitOfWork.GetRepository<Product, int>();

                var product = await repository.GetByIdAsync(productId);

                if (product == null)
                    return Error.NotFound("غير_موجود", "المنتج المطلوب غير موجود في النظام.");

                product.Discount = newDiscount;

                repository.Update(product);
                await _unitOfWork.SaveChangesAsync();

                return Result.Ok();
           
        }

        public async Task<Result> UpdateImageProductAsync(int productId, IFormFile image)
        {
            if (image == null || image.Length == 0)
                return Error.Validation("بيانات_غير_صالحة", "يرجى إرفاق صورة صالحة.");

            string? uploadedFilePath = null;
            string? oldFilePath = null;

            async Task RevertUploadAsync()
            {
                if (!string.IsNullOrEmpty(uploadedFilePath))
                {
                    await _fileService.DeleteAsync(uploadedFilePath);
                }
            }

            try
            {
                var repository = _unitOfWork.GetRepository<Product, int>();
                var product = await repository.GetByIdAsync(productId);

                if (product == null)
                    return Error.NotFound("غير_موجود", "المنتج المطلوب تعديل صورته غير موجود.");

               
                var uploadResult = await _fileService.SaveFileAsync(image, "ProductImage");
                if (!uploadResult.IsSuccess)
                    return Error.Failure("حدث خطأ اثناء حفظ الصوره ", "حاول ترفع صوره اخرى ");

                uploadedFilePath = uploadResult.Value;

               
                oldFilePath = product.ImageUrl;
                product.ImageUrl = uploadedFilePath;

                
                repository.Update(product);
                await _unitOfWork.SaveChangesAsync();

          
                if (!string.IsNullOrEmpty(oldFilePath))
                {
                    await _fileService.DeleteAsync(oldFilePath);
                }

                return Result.Ok();
            }
            catch (Exception ex)
            {
            
                await RevertUploadAsync();

               // _logger.LogError(ex, $"Error updating image for product {productId}...");
                return Error.Failure("خطأ_غير_متوقع", "حدث خطأ غير متوقع أثناء تحديث صورة المنتج.");
            }
        }

        public async Task<Result<IEnumerable<GetTopProductDTO>>> GetTopProductsAsync()
        {
            
                var repository = _unitOfWork.GetRepository<Product, int>();

                var sp = new ProductSpecification();
                var allProducts = await repository.GetAllAsync(sp);
                
                var dtos = _mapper.Map<IEnumerable<GetTopProductDTO>>(allProducts);

                return Result<IEnumerable<GetTopProductDTO>>.Ok(dtos);
           
        }
    }
}
