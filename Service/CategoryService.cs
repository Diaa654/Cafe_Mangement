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
    public class CategoryService(IUnitOfWork _unitOfWork) : ICategoryService
    {
        public async Task<Result> AddAsync(CategoryDTO dto)
        {
            if (dto == null)
                return Error.Validation("يجب ادخال البيانات ","يجب ادخال البيانات ");

          
                var Category = new Category()
                {
                    Name = dto.Name,
                };
                await _unitOfWork.GetRepository<Category, int>().AddAsync(Category);
                await _unitOfWork.SaveChangesAsync();
                return Result.Ok();
            

        }

        public async Task<Result<IEnumerable<CategoryDTO>>> GetAllCategoriesAsync()
        {
          
                var categories = await _unitOfWork.GetRepository<Category, int>().GetAllAsync();
                var dtos = categories.Select(c => new CategoryDTO
                {
                    Name = c.Name,
                    ID= c.Id

                });

                return Result<IEnumerable<CategoryDTO>>.Ok(dtos);
           
        }
        public async Task<Result> DeleteAsync(int categoryId)
        {
            
                var repository = _unitOfWork.GetRepository<Category, int>();
                var existingCategory = await repository.GetByIdAsync(categoryId);

                if (existingCategory == null)
                    return Error.NotFound("غير_موجود", "القسم المطلوب حذفه غير موجود.");

                repository.Remove(existingCategory);
                await _unitOfWork.SaveChangesAsync();

                return Result.Ok();
           
        }

        public async Task<Result> UpdateAsync(int categoryId, CategoryDTO dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Name))
                return Error.Validation("يجب ادخال البيانات", "اسم القسم مطلوب.");

           
                var repository = _unitOfWork.GetRepository<Category, int>();
                var existingCategory = await repository.GetByIdAsync(categoryId);

                if (existingCategory == null)
                    return Error.NotFound("غير_موجود", "القسم المطلوب تعديله غير موجود.");

                
                existingCategory.Name = dto.Name;

                repository.Update(existingCategory);
                await _unitOfWork.SaveChangesAsync();

                return Result.Ok();
           
        }
    }
}
