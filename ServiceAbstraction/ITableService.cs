using Shared.CommonResult;
using Shared.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbstraction
{
    public interface ITableService
    {
        Task<Result<IEnumerable<GetAllTableDTO>>> GetAllTablesAsync();
        Task<Result<IEnumerable<GetAllTableDTO>>> GetAllTableAvailablesAsync();
        Task<Result> AddTable();
        Task<Result> UpdateTableAvailability(int tableId, bool isAvailable);
    }
}
