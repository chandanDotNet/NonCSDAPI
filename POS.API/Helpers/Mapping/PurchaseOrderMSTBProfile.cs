using AutoMapper;
using POS.Data.Dto;
using POS.Data;
using POS.MediatR.CommandAndQuery;
using POS.MediatR.PurchaseOrderMSTB.Command;

namespace POS.API.Helpers.Mapping
{
    public class PurchaseOrderMSTBProfile : Profile
    {
        public PurchaseOrderMSTBProfile()
        {
            CreateMap<MSTBPurchaseOrder, MSTBPurchaseOrderDto>().ReverseMap();
            CreateMap<AddPurchaseOrderMSTBCommand, MSTBPurchaseOrder>();
            CreateMap<MSTBPurchaseOrderItem, MSTBPurchaseOrderItemDto>().ReverseMap();
            CreateMap<MSTBPurchaseOrderItemTax, MSTBPurchaseOrderItemTaxDto>().ReverseMap();
            CreateMap<UpdatePurchaseOrderMSTBCommand, MSTBPurchaseOrder>();

            CreateMap<MSTBPurchaseOrderPayment, MSTBPurchaseOrderPaymentDto>().ReverseMap();

        }
    }
}
