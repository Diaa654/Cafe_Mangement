using AutoMapper;
using Domain.Contracts;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ServiceAbstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class ServiceManger(IUnitOfWork _unitOfWork, IMapper _mapper, 
        
        IFileService _fileService, UserManager<User> _userManager,
        ILogger<FcmService> _loggerfcm,
        IFcmService _fcmService,ICacheRepository _cacheRepository, IDashboardNotificationService _dashboardNotification) : IServiceManger
    {
        private readonly Lazy<ITableService> _LazyTableService = new Lazy<ITableService>(() => new TableService(_unitOfWork, _mapper));
        public ITableService TableService => _LazyTableService.Value;

        private readonly Lazy<IProductService> _LazyProductService = new Lazy<IProductService>(() => new ProductService(_unitOfWork,_fileService, _mapper));
        public IProductService ProductService => _LazyProductService.Value;

        private readonly Lazy<ICategoryService> _LazyCategoryService = new Lazy<ICategoryService>(() => new CategoryService(_unitOfWork));
        public ICategoryService CategoryService => _LazyCategoryService.Value;
        
        private readonly Lazy<IInvoiceService> _LazyInvoiceService = new Lazy<IInvoiceService>(() => new InvoiceService(_unitOfWork, _mapper, _userManager,_cacheRepository,_dashboardNotification));
        public IInvoiceService InvoiceService => _LazyInvoiceService.Value;

        private readonly Lazy<IOrderService> _LazyOrderService = new Lazy<IOrderService>(() => new OrderService(_unitOfWork, _mapper,_fcmService,_userManager));
        public IOrderService OrderService => _LazyOrderService.Value;
        private readonly Lazy<IFcmService> _LazyFcmService = new Lazy<IFcmService>(() => new FcmService(_loggerfcm));
        public IFcmService FcmService => _LazyFcmService.Value;

        
    }
}
