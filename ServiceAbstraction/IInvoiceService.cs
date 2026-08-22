using Shared;
using Shared.CommonResult;
using Shared.DTOS;
using Shared.DTOS.InvoiceDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbstraction
{
    public interface IInvoiceService
    {
        Task<Result<int>> CreateInvoiceAsync(int tableId, int userId);
        Task<Result<InvoiceDetailsDto>> GetInvoiceDetailsByTableIdAsync(int tableId,int userId);
        Task<Result<InvoiceDetailsWithStatusLog>> GetInvoiceDetailsByInvoiceIdAsync(int invoiceID, int userId);
        Task<Result<InvoiceDetailsDto>> CloseInvoiceAsync(int invoiceId,int userId);
        Task<Result<PaginatedResult<InvoiceDto>>> GetAllInvoicesAsync(InvoiceSpecParams specParams,int userId);
    }
}
