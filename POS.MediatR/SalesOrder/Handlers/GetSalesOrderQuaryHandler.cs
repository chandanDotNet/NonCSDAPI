using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using POS.Data.Dto;
using POS.Helper;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Handlers
{
    public class GetSalesOrderQuaryHandler : IRequestHandler<GetSalesOrderCommand, ServiceResponse<SalesOrderDto>>
    {
        private readonly ISalesOrderRepository _salesOrderRepository;
        private readonly IMapper _mapper;

        public GetSalesOrderQuaryHandler(ISalesOrderRepository salesOrderRepository,
            IMapper mapper)
        {
            _salesOrderRepository = salesOrderRepository;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<SalesOrderDto>> Handle(GetSalesOrderCommand request, CancellationToken cancellationToken)
        {
            var entity = await _salesOrderRepository.All
                 .Include(c => c.SalesOrderPayments)
                 .Include(c => c.Customer)
                 .Include(c => c.SalesOrderItems)
                 .ThenInclude(c => c.SalesOrderItemTaxes)
                 .ThenInclude(cs => cs.Tax)
                 .Include(c => c.SalesOrderItems)
                 .ThenInclude(c => c.Product)
                 .ThenInclude(cs => cs.ProductCategory)
                 .Include(c => c.SalesOrderItems)
                 .ThenInclude(c => c.UnitConversation)
                 .Include(c => c.Customer)
                 .ThenInclude(cs => cs.CustomerAddresses)
                //.Include(c => c.DeliveryAddress)
                .Where(c => c.Id == request.Id)
                .FirstOrDefaultAsync();
            if (entity == null)
            {
                return ServiceResponse<SalesOrderDto>.Return404();
            }
            else
            {
                if (entity.Customer.CustomerAddresses.Count > 0)
                {
                    //var customerAddress = entity.Customer.CustomerAddresses.Where(x => x.IsPrimary == true).FirstOrDefault();
                    //entity.DeliveryAddress = customerAddress.HouseNo + " " +
                    //                         customerAddress.StreetDetails + " " +
                    //                         customerAddress.LandMark + " " +
                    //                         customerAddress.Pincode;
                    entity.DeliveryAddress = entity.Customer.CustomerAddresses.Where(c => c.IsPrimary == true)
                   .Select(x => x.HouseNo + " " + x.StreetDetails + ", " + x.LandMark + " - " + x.Pincode)
                   .FirstOrDefault();
                }
                else
                {
                    entity.DeliveryAddress = entity.Customer.Address;
                }
                if (entity.SalesOrderItems.Count > 0)
                {
                    for (int i = 0; i < entity.SalesOrderItems.Count; i++)
                    {
                        decimal value = (decimal)(entity.SalesOrderItems[i].UnitPrice) * entity.SalesOrderItems[i].Quantity;
                        int roundedValue = (int)Math.Round(value, MidpointRounding.AwayFromZero);
                        entity.SalesOrderItems[i].TotalSalesPrice = roundedValue;
                        //entity.SalesOrderItems[i].TotalSalesPrice = decimal.Round((decimal)(entity.SalesOrderItems[i].UnitPrice * entity.SalesOrderItems[i].Quantity));
                    }
                }
            }
            var dto = _mapper.Map<SalesOrderDto>(entity);
            dto.BillNo = dto.OrderNumber.Substring(3, dto.OrderNumber.Length - 3);
            return ServiceResponse<SalesOrderDto>.ReturnResultWith200(dto);
        }
    }
}
