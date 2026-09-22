using AutoMapper;
using Domain.Contracts;
using Domain.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Service.Specifications;
using ServiceAbstraction;
using Shared;
using Shared.CommonResult;
using Shared.DTOS.InvoiceDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InvoiceStatus = Domain.Models.InvoiceStatus;
using OrderStatus = Domain.Models.OrderStatus;
using Table = Domain.Models.Table;

namespace Service
{
    public class InvoiceService(IUnitOfWork _unitOfWork, IMapper _mapper
        , UserManager<User> _userManager,ICacheRepository _cacheRepository
        , IDashboardNotificationService _dashboardNotificationService) : IInvoiceService
    {
        public async Task<Result<int>> CreateInvoiceAsync(int tableId, int userId)
        {
            var userRepo= _unitOfWork.GetRepository<User,int>();
            var tableRepo= _unitOfWork.GetRepository<Table,int>();
            var user =await userRepo.GetByIdAsync(userId);
            if(user == null)
                return Error.Failure("User not found","الحساب غير موجود فى قاعدة البيانات");
            if(!user.IsActive)
                return Error.Failure("User is not active","الحساب غير مفعل حاليا");
            var userRoles = (await _userManager.GetRolesAsync(user)).ToList();
            //if (!userRoles.Contains("Waiter"))
            //    return Error.Failure("User is not a waiter","غير مسموح ليك بانشاء فاتورة");
            if (!user.IsActive)
                return Error.Failure("User is not active","الحساب غير مفعل حاليا");
            var table =await tableRepo.GetByIdAsync(tableId);
            if(table == null)
                return Error.Failure("Table not found","رقم الطاولة غير صحيح");
            if(!table.IsAvailable)
                return Error.Failure("Table is not available","الطاولة غير متاحة حاليا");
            var invoiceSpec= new InvoiceSpecifications(tableId);
            var invoiceRepo= _unitOfWork.GetRepository<Invoice,int>();
            var existingInvoice =await invoiceRepo.GetByIdAsync(invoiceSpec);
            if(existingInvoice!=null)
                return Error.Failure("Invoice already exists", "يوجد فاتورة مفتوحة لهذه الطاولة بالفعل");
            var newInvoice = new Invoice
            {
                TableId = tableId,
                UserId = userId,
                Status = InvoiceStatus.Pending,
                TotalAmount = 0
            };
            await invoiceRepo.AddAsync(newInvoice);
            table.IsAvailable = false;
            tableRepo.Update(table);
            await _unitOfWork.SaveChangesAsync();
            return Result<int>.Ok(newInvoice.Id);
        }
        public async Task<Result<InvoiceDetailsDto>> GetInvoiceDetailsByTableIdAsync(int tableId,int userId)
        {
            var user = await _unitOfWork.GetRepository<User,int>().GetByIdAsync(userId);
            if(user == null)
                return Error.Failure("User not found","المستخدم غير موجود فى قاعدة البيانات");
            if(!user.IsActive)
                return Error.Failure("User is not active","المستخدم غير مفعل حاليا");
            var table = await _unitOfWork.GetRepository<Table,int>().GetByIdAsync(tableId);
            if (table == null)
                return Error.Failure("Table not found", "رقم الطاولة غير صحيح");
            var invoiceSpec = new InvoiceSpecifications(tableId);
            var invoiceRepo = _unitOfWork.GetRepository<Invoice,int>();
            var invoice = await invoiceRepo.GetByIdAsync(invoiceSpec);
            if (invoice == null)
                return Error.Failure("Invoice not found", "لا توجد فاتورة مفتوحة لهذه الطاولة");
            var invoiceDto = _mapper.Map<InvoiceDetailsDto>(invoice);
            
            return Result<InvoiceDetailsDto>.Ok(invoiceDto);


        }
        public async Task<Result<InvoiceDetailsDto>> CloseInvoiceAsync(int invoiceId,int userId)
        {
            var user = await _unitOfWork.GetRepository<User,int>().GetByIdAsync(userId);
            if(user == null)
                return Error.Failure("User not found","المستخدم غير موجود فى قاعدة البيانات");
            if(!user.IsActive)
                return Error.Failure("User is not active","المستخدم غير مفعل حاليا");
            var spec=new CloseInvoiceSpecifications(invoiceId);
            var invoice = await _unitOfWork.GetRepository<Invoice, int>().GetByIdAsync(spec);
            if (invoice == null)
                return Error.Failure("Invoice not found", "خطأ فى رقم الفاتورة");
            if(invoice.Status == InvoiceStatus.Paid)
                return Error.Failure("Invoice already closed", "تم استلام حساب الفاتورة بالفعل");
            invoice.Status = InvoiceStatus.Paid;
            var table = await _unitOfWork.GetRepository<Table, int>().GetByIdAsync(invoice.TableId);
            table!.IsAvailable = true;
            foreach(var order in invoice.Orders)
            {
                order.Status = OrderStatus.Completed;
            }
            invoice.TotalAmount = invoice.Orders.SelectMany(o => o.OrderItems).Sum(oi => oi.TotalPrice);
            int result = await _unitOfWork.SaveChangesAsync();
            if(result <= 0)
                return Error.Failure("Failed to close invoice", "فشل فى استلام حساب الفاتورة");
            await _cacheRepository.IncrementAmountAsync("Total_financial_collection", (double)invoice.TotalAmount);
            await _cacheRepository.IncrementCountAsync("Total_invoices_count");
            var updatedAmount = await _cacheRepository.GetAsync("Total_financial_collection");
            var updatedCount = await _cacheRepository.GetAsync("Total_invoices_count");
            await _dashboardNotificationService.SendDashboardUpdatesAsync(updatedAmount, (int)updatedCount);
            var invoiceDto = _mapper.Map<InvoiceDetailsDto>(invoice);
            return Result<InvoiceDetailsDto>.Ok(invoiceDto);
        }

        public async Task<Result<PaginatedResult<InvoiceDto>>> GetAllInvoicesAsync(InvoiceSpecParams specParams,int userId)
        {
            var user = await _unitOfWork.GetRepository<User,int>().GetByIdAsync(userId);
            if(user == null)
                return Error.Failure("User not found","المستخدم غير موجود فى قاعدة البيانات");
            if(!user.IsActive)
                return Error.Failure("User is not active","المستخدم غير مفعل حاليا");
            var spec=new InvoiceSpecifications(specParams);
            var invoiceRepo = _unitOfWork.GetRepository<Invoice, int>();
            var invoices = await invoiceRepo.GetAllAsync(spec);
            var invoicesDtos = _mapper.Map<IEnumerable<InvoiceDto>>(invoices);
            var totalCount = await invoiceRepo.CountAsync(spec);
            var paginatedResult = new PaginatedResult<InvoiceDto>(specParams.PageIndex, invoicesDtos.Count(), totalCount, invoicesDtos);
            return Result<PaginatedResult<InvoiceDto>>.Ok(paginatedResult);
        }

        public async Task<Result<InvoiceDetailsWithStatusLog>> GetInvoiceDetailsByInvoiceIdAsync(int invoiceId,int userId)
        {
            var user = await _unitOfWork.GetRepository<User,int>().GetByIdAsync(userId);
            if(user == null)
                return Error.Failure("User not found","المستخدم غير موجود فى قاعدة البيانات");
            if(!user.IsActive)
                return Error.Failure("User is not active","المستخدم غير مفعل حاليا");
            var spec = new InvoiceDetailsSpecifications(invoiceId);
            var invoice = await _unitOfWork.GetRepository<Invoice, int>().GetByIdAsync(spec);
            if (invoice == null)
                return Error.Failure("Invoice not found", "خطأ فى رقم الفاتورة");
            var invoiceDto = _mapper.Map<InvoiceDetailsWithStatusLog>(invoice);
              return Result<InvoiceDetailsWithStatusLog>.Ok(invoiceDto);

        }
    }
}
