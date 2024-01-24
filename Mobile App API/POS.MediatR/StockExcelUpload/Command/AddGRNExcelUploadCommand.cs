using MediatR;
using Microsoft.AspNetCore.Http;
using POS.Data.Dto;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.MediatR.StockExcelUpload.Command
{
    public class AddGRNExcelUploadCommand : IRequest<ServiceResponse<StockExcelUploadDto>>
    {

        public IFormFile FileDetails { get; set; }
    }
}
