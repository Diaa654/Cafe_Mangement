using AutoMapper;
using Domain.Contracts;
using Domain.Models;
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
    public class TableService(IUnitOfWork _unitOfWork, IMapper _mapper, ILogger _logger) : ITableService
    {
        public async Task<Result> AddTable()
        {
            try
            {
 
                var table = new Table()
                {
                    IsAvailable = true
                };

                await _unitOfWork.GetRepository<Table, int>().AddAsync(table);
                await _unitOfWork.SaveChangesAsync();

                return Result.Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while adding a new Table...");
                return Error.Failure("فشل_النظام", "حدث خطأ غير متوقع أثناء إضافة الطاولة.");
            }
        }

        public async Task<Result<IEnumerable<GetAllTableDTO>>> GetAllTableAvailablesAsync()
        {
            try
            {
                var repository = _unitOfWork.GetRepository<Table, int>();
                var sp = new TableSpecification();
                var allTables = await repository.GetAllAsync(sp);
                var dtos = allTables.Select(t => new GetAllTableDTO
                {
                    Id = t.Id,
                    IsAvailable = t.IsAvailable
                });

                return Result<IEnumerable<GetAllTableDTO>>.Ok(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting available tables...");
                return Error.Failure("فشل_النظام", "حدث خطأ أثناء جلب الطاولات المتاحة.");
            }
        }

        public async Task<Result<IEnumerable<GetAllTableDTO>>> GetAllTablesAsync()
        {
            try
            {
                var repository = _unitOfWork.GetRepository<Table, int>();
                var allTables = await repository.GetAllAsync();
                var dtos = allTables.Select(t => new GetAllTableDTO
                {
                    Id = t.Id,
                    IsAvailable = t.IsAvailable
                });

                return Result<IEnumerable<GetAllTableDTO>>.Ok(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting all tables...");
                return Error.Failure("فشل_النظام", "حدث خطأ أثناء جلب الطاولات.");
            }
        }

        public async Task<Result> UpdateTableAvailability(int tableId, bool isAvailable)
        {
            try
            {
                var repository = _unitOfWork.GetRepository<Table, int>();

                var table = await repository.GetByIdAsync(tableId);
                if (table == null)
                    return Error.NotFound("غير_موجود", "الطاولة المطلوبة غير موجودة في النظام.");

                table.IsAvailable = isAvailable;

                repository.Update(table);
                await _unitOfWork.SaveChangesAsync();

                return Result.Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating availability for table {tableId}...");
                return Error.Failure("فشل_النظام", "حدث خطأ أثناء تغيير حالة الطاولة.");
            }
        }
    }
}
