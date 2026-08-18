using Domain.Contracts;
using Domain.Models;
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
    public class CategoryService(IUnitOfWork _unitOfWork,ILogger _logger) : ICategoryService
    {
        public async Task<Result> AddAsync(CategoryDTO dto)
        {
            if (dto == null)
                return Error.Validation("يجب ادخال البيانات ","يجب ادخال البيانات ");

            try
            {
                var Category = new Category()
                {
                    Name = dto.Name,
                };
                await _unitOfWork.GetRepository<Category, int>().AddAsync(Category);
                await _unitOfWork.SaveChangesAsync();
                return Result.Ok();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Error.Failure("فشل_النظام", ex.Message);
            }

        }

        public async Task<Result<IEnumerable<CategoryDTO>>> GetAllCategoriesAsync()
        {
            try
            {
                var categories = await _unitOfWork.GetRepository<Category, int>().GetAllAsync();
                var dtos = categories.Select(c => new CategoryDTO
                {
                    Name = c.Name
                    
                });

                return Result<IEnumerable<CategoryDTO>>.Ok(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting all categories...");
                return Error.Failure("فشل_النظام", "حدث خطأ أثناء جلب الأقسام.");
            }
        }
        public async Task<Result> DeleteAsync(int categoryId)
        {
            try
            {
                var repository = _unitOfWork.GetRepository<Category, int>();
                var existingCategory = await repository.GetByIdAsync(categoryId);

                if (existingCategory == null)
                    return Error.NotFound("غير_موجود", "القسم المطلوب حذفه غير موجود.");

                repository.Remove(existingCategory);
                await _unitOfWork.SaveChangesAsync();

                return Result.Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while deleting category with ID {categoryId}...");
                return Error.Failure("فشل_النظام", "حدث خطأ أثناء حذف القسم. تأكد أنه غير مرتبط بمنتجات أخرى.");
            }
        }

        public async Task<Result> UpdateAsync(int categoryId, CategoryDTO dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Name))
                return Error.Validation("يجب ادخال البيانات", "اسم القسم مطلوب.");

            try
            {
                var repository = _unitOfWork.GetRepository<Category, int>();
                var existingCategory = await repository.GetByIdAsync(categoryId);

                if (existingCategory == null)
                    return Error.NotFound("غير_موجود", "القسم المطلوب تعديله غير موجود.");

                
                existingCategory.Name = dto.Name;

                repository.Update(existingCategory);
                await _unitOfWork.SaveChangesAsync();

                return Result.Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while updating category with ID {categoryId}...");
                return Error.Failure("فشل_النظام", "حدث خطأ أثناء تعديل بيانات القسم.");
            }
        }
    }
}
