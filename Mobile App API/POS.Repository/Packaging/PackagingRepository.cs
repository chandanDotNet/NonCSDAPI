using POS.Common.GenericRepository;
using POS.Common.UnitOfWork;
using POS.Data;
using POS.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Repository
{
    public class PackagingRepository : GenericRepository<Packaging, POSDbContext>, IPackagingRepository
    {
        public PackagingRepository(IUnitOfWork<POSDbContext> uow)
            : base(uow)
        {
        }
    }
}
