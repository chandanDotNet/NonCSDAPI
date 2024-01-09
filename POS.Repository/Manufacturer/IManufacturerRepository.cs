using POS.Common.GenericRepository;
using POS.Data;
using POS.Data.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Repository
{
    public interface IManufacturerRepository : IGenericRepository<Manufacturer>
    {
        Task<ManufacturerList> GetManufacturers(ManufacturerResource manufacturerResource);
    }
}
