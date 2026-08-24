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
    public class TableService(IUnitOfWork _unitOfWork, IMapper _mapper) : ITableService
    {
        public async Task<Result> AddTable()
        {
            
                var table = new Table()
                {
                    IsAvailable = true
                };

                await _unitOfWork.GetRepository<Table, int>().AddAsync(table);
                await _unitOfWork.SaveChangesAsync();

                return Result.Ok();
           
        }

        public async Task<Result<IEnumerable<GetAllTableDTO>>> GetAllTableAvailablesAsync()
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

        public async Task<Result<IEnumerable<GetAllTableDTO>>> GetAllTablesAsync()
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
        

        public async Task<Result> UpdateTableAvailability(int tableId, bool isAvailable)
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
        public async Task<Result<IEnumerable<TableDetailsDto>>> GetAllTablesWithDetails(int userId)
        {
            var user = await _unitOfWork.GetRepository<User, int>().GetByIdAsync(userId);
            if (user == null)
                return Error.Failure("User Not Found", "المستخدم غير موجود.");
            if (!user.IsActive)
                return Error.Failure("User Inactive", "لا يمكنك إجراء أي نشاط في الشيفت الحالي");
            var spec =new TableWithDetailsSpecifications();
            var repository = _unitOfWork.GetRepository<Table, int>();
            var tables = await repository.GetAllAsync(spec);
            var tablesWithDetailsDto = _mapper.Map<IEnumerable<TableDetailsDto>>(tables);
            return Result<IEnumerable<TableDetailsDto>>.Ok(tablesWithDetailsDto);
            
        }
    }
}
