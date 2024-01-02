using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualBasic.FileIO;
using POS.Data.Dto;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.MediatR.ExcelUpload.Command
{
    public class AddStockExcelUploadCommand : IRequest<ServiceResponse<StockExcelUploadDto>>
    {
        public IFormFile FileDetails { get; set; }

    }
}
