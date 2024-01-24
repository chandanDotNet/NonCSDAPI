using POS.Common.GenericRepository;
using POS.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POS.Common.UnitOfWork;
using POS.Data;
using AutoMapper;
using POS.Data.Dto;
using Microsoft.IdentityModel.Tokens;
using POS.Data.Resources;

namespace POS.Repository
{
    public class CartRepository : GenericRepository<Cart, POSDbContext>, ICartRepository
    {

        private readonly IPropertyMappingService _propertyMappingService;
        private readonly IMapper _mapper;
        private readonly ISalesOrderRepository _salesOrderRepository;

        public CartRepository(IUnitOfWork<POSDbContext> uow,
            IPropertyMappingService propertyMappingService,
            IMapper mapper,
            ISalesOrderRepository salesOrderRepository)
            : base(uow)
        {
            _propertyMappingService = propertyMappingService;
            _mapper = mapper;
            _salesOrderRepository = salesOrderRepository;
        }

        public async Task<CartList> GetCartsData(CartResource cartResource)
        {
            var collectionBeforePaging =
                 AllIncluding(c => c.Product, i => i.Product.Inventory, p => p.Product.ProductCategory, m => m.Product.ProductCategory.ProductMainCategory).ApplySort(cartResource.OrderBy,
                 _propertyMappingService.GetPropertyMapping<CartDto, Cart>()).Where(a => a.CustomerId == cartResource.CustomerId && a.ProductMainCategoryId == cartResource.ProductMainCategoryId);


            //collectionBeforePaging = collectionBeforePaging
            //    .Where(a => a.Product.OrderEndTime == string.Empty ||
            //    a.Product.OrderEndTime == null ||
            //    (
            //    Convert.ToInt32(a.Product.OrderEndTime.Substring(0, 2)
            //    + a.Product.OrderEndTime.Substring(3, 4)) >=
            //    Convert.ToInt32(DateTime.Now.Hour.ToString() +
            //    (DateTime.Now.Minute.ToString().Length > 1 ? DateTime.Now.Minute.ToString()
            //    : "0" + DateTime.Now.Minute.ToString()))));

            //if (cartResource.CustomerId!= Guid.Empty)
            //{
            //    collectionBeforePaging = collectionBeforePaging
            //        .Where(a => a.CustomerId == cartResource.CustomerId);
            //}

            var CartList = new CartList(_mapper);
            return await CartList.Create(collectionBeforePaging,
                cartResource.Skip,
                cartResource.PageSize);
        }
    }
}
