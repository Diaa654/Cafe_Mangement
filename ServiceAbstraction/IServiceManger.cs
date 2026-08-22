using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbstraction
{
    public interface IServiceManger
    {
        public ITableService TableService { get; }
        public IProductService ProductService { get; }
        public ICategoryService CategoryService { get; }
        public IInvoiceService InvoiceService { get; }
        public IOrderService OrderService { get; }
        public IFcmService FcmService { get; }
    }
}
