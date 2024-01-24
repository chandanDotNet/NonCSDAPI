using POS.Data.Dto;
using POS.Data;
using POS.MediatR.CommandAndQuery;
using POS.MediatR.PurchaseOrder.Commands;
using POS.MediatR.PurchaseOrderPayment.Command;
using AutoMapper;
using POS.Data.Dto.GRN;
using POS.Data.Entities.GRN;
using POS.MediatR.GRN.Command;

namespace POS.API.Helpers.Mapping
{
    public class GRNProfile:Profile
    {

        public GRNProfile()
        {
            CreateMap<GRN, GRNDto>().ReverseMap();
            CreateMap<AddGRNCommand, GRN>();
            CreateMap<GRNItem, GRNItemDto>().ReverseMap();
            CreateMap<GRNItemTax, GRNItemTaxDto>().ReverseMap();
            CreateMap<UpdateGRNCommand, GRN>();

            //CreateMap<PurchaseOrderPayment, PurchaseOrderPaymentDto>().ReverseMap();
            //CreateMap<AddPurchaseOrderPaymentCommand, PurchaseOrderPayment>();
            //CreateMap<UpdatePurchaseOrderReturnCommand, PurchaseOrder>();
        }

    }
}
