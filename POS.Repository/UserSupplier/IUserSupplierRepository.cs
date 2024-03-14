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
    public interface IUserSupplierRepository : IGenericRepository<UserSupplier>
    {
        Task<UserSupplierList> GetUserSuppliers(UserSupplierResource userSupplierResource);
    }
}
