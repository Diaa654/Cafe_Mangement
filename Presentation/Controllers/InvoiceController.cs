using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceAbstraction;
using Shared;
using Shared.DTOS.InvoiceDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    public class InvoiceController(IServiceManger serviceManger): ApiBaseController
    {
        [HttpPost("CreateInvoice/{tableId}")]
        [Authorize(Roles = nameof(AppRoles.Waiter) + "," + nameof(AppRoles.Barista))]
        public async Task<ActionResult<int>> CreateInvoice(int tableId)
        {
            var userId = GetUserId();
            var result = await serviceManger.InvoiceService.CreateInvoiceAsync(tableId, userId);
            return HandleResult(result);
        }
        [HttpGet("GetInvoiceDetailsByTableId/{tableId}")]
        [Authorize(Roles = nameof(AppRoles.Waiter) + "," + nameof(AppRoles.Barista))]
        public async Task<ActionResult<InvoiceDetailsDto>> GetInvoiceDetailsByTableId(int tableId)
        {
            var userId = GetUserId();
            var result = await serviceManger.InvoiceService.GetInvoiceDetailsByTableIdAsync(tableId, userId);
            return HandleResult(result);
        }
        [HttpPut("CloseInvoiceByInvoiceId/{InvoiceId}")]
        [Authorize(Roles = nameof(AppRoles.Barista))]
        public async Task<ActionResult<InvoiceDetailsDto>> CloseInvoiceByInvoiceId(int InvoiceId)
        {
            var userId = GetUserId();
            var result = await serviceManger.InvoiceService.CloseInvoiceAsync(InvoiceId, userId);
            return HandleResult(result);
        }
        [HttpGet("GetInvoiceDetailsByInvoiceId/{invoiceId}")]
        [Authorize(Roles = nameof(AppRoles.Admin))]
        public async Task<ActionResult<InvoiceDetailsWithStatusLog>> GetInvoiceDetailsByInvoiceId(int invoiceId)
        {
            var userId = GetUserId();
            var result = await serviceManger.InvoiceService.GetInvoiceDetailsByInvoiceIdAsync(invoiceId, userId);
            return HandleResult(result);
        }
        [HttpGet("GetAllInvoices")]
        [Authorize(Roles = nameof(AppRoles.Admin))]
        public async Task<ActionResult<PaginatedResult<InvoiceDto>>> GetAllInvoices([FromQuery] InvoiceSpecParams invoiceSpec)
        {
            var userId = GetUserId();
            var result = await serviceManger.InvoiceService.GetAllInvoicesAsync(invoiceSpec, userId);
            return HandleResult(result);
        }
    }
}
