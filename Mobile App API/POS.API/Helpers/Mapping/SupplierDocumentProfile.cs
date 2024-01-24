using AutoMapper;
using POS.Data.Dto;
using POS.Data;
using POS.MediatR.Supplier.Commands;

namespace POS.API.Helpers.Mapping
{
    public class SupplierDocumentProfile : Profile
    {
        public SupplierDocumentProfile()
        {
            CreateMap<SupplierDocument, SupplierDocumentDto>().ReverseMap();
            CreateMap<AddSupplierDocumentCommand, SupplierDocument>();
            //CreateMap<SupplierDocumentDto, SupplierDocument>();
        }
    }
}
