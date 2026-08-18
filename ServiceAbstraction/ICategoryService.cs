using Shared.CommonResult;
using Shared.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbstraction
{
    public interface ICategoryService
    {
        Task<Result<IEnumerable<CategoryDTO>>> GetAllCategoriesAsync();
        Task<Result> AddAsync(CategoryDTO dto);
        Task<Result> DeleteAsync(int CategoryId);

        Task<Result> UpdateAsync(int CategoryId ,CategoryDTO dto);

    }
}
