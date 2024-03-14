using MediatR;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.UserSupplier.Handler
{
    internal class SearchUserSupplierQueryHandler : IRequestHandler<SearchUserSupplierQuery, UserSupplierList>
    {
        private readonly IUserSupplierRepository _userSupplierRepository;

        public SearchUserSupplierQueryHandler(IUserSupplierRepository userSupplierRepository)
        {
            _userSupplierRepository = userSupplierRepository;
        }
        public async Task<UserSupplierList> Handle(SearchUserSupplierQuery request, CancellationToken cancellationToken)
        {           
            return await _userSupplierRepository.GetUserSuppliers(request.UserSupplierResource);
        }
    }
}