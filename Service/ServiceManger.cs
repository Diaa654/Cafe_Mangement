using AutoMapper;
using Domain.Contracts;
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
    public class ServiceManger(IUnitOfWork _unitOfWork, IMapper _mapper, ILogger _logger,IFileService _fileService) : IServiceManger
    {
        private readonly Lazy<ITableService> _LazyTableService = new Lazy<ITableService>(() => new TableService(_unitOfWork, _mapper, _logger));
        public ITableService TableService => _LazyTableService.Value;

        private readonly Lazy<IProductService> _LazyProductService = new Lazy<IProductService>(() => new ProductService(_unitOfWork,_fileService, _mapper, _logger));
        public IProductService ProductService => _LazyProductService.Value;

        private readonly Lazy<ICategoryService> _LazyCategoryService = new Lazy<ICategoryService>(() => new CategoryService(_unitOfWork, _logger));
        public ICategoryService CategoryService => _LazyCategoryService.Value;

        
    }
}
