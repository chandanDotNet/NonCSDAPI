using AutoMapper;
using POS.Data.Dto;
using POS.Data;
using POS.MediatR.Batch.Command;

namespace POS.API.Helpers.Mapping
{
    public class BatchProfile : Profile
    {
        public BatchProfile()
        {
            CreateMap<Batch, BatchDto>().ReverseMap();
            CreateMap<AddBatchCommand, Batch>();
        }
    }
}
