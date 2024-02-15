using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Data.Dto;
using POS.Data;
using POS.Domain;
using POS.Helper;
using POS.MediatR.CommandAndQuery;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using POS.Repository;
using Microsoft.EntityFrameworkCore;

namespace POS.MediatR.Handlers
{
    public class UpdateAppUserProfileCommandHandler : IRequestHandler<UpdateAppUserProfileCommand, ServiceResponse<UserDto>>
    {
        IUserRoleRepository _userRoleRepository;
        private readonly IUserRepository _userRepository;
        private readonly UserManager<User> _userManager;
        IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private UserInfoToken _userInfoToken;
        private readonly ILogger<UpdateUserProfileCommandHandler> _logger;
        public readonly PathHelper _pathHelper;
        public UpdateAppUserProfileCommandHandler(
            IUserRepository userRepository,
            IMapper mapper,
            IUnitOfWork<POSDbContext> uow,
            UserInfoToken userInfoToken,
            UserManager<User> userManager,
            ILogger<UpdateUserProfileCommandHandler> logger,
            PathHelper pathHelper,
            IUserRoleRepository userRoleRepository
            )
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _userManager = userManager;
            _uow = uow;
            _userInfoToken = userInfoToken;
            _logger = logger;
            _pathHelper = pathHelper;
            _userRoleRepository = userRoleRepository;
        }

        public async Task<ServiceResponse<UserDto>> Handle(UpdateAppUserProfileCommand request, CancellationToken cancellationToken)
        {
            var userInfo = await _userRepository
                .All.Include(x => x.Counter)
                .Where(c => c.PhoneNumber == request.MobileNumber)
                .FirstOrDefaultAsync();

            var userRoles = _userRoleRepository
              .AllIncluding(c => c.User)
              .Where(c => c.UserId == userInfo.Id)
              .Select(cs => new UserRoleDto
              {
                  UserId = cs.UserId,
                  RoleId = cs.RoleId,
                  UserName = cs.User.UserName,
                  FirstName = cs.User.FirstName,
                  LastName = cs.User.LastName
              }).ToList();

            var appUser = await _userManager.FindByIdAsync(Convert.ToString(userInfo.Id));

            if (appUser == null && userRoles.Count == 0)
            {
                _logger.LogError("User does not exist.");
                return ServiceResponse<UserDto>.Return409("User does not exist.");
            }

            int _min = 1000;
            int _max = 9999;
            Random rnd = new Random();
            appUser.Otp = rnd.Next(_min, _max);
          
            IdentityResult result = await _userManager.UpdateAsync(appUser);
            if (await _uow.SaveAsync() <= 0 && !result.Succeeded)
            {
                return ServiceResponse<UserDto>.Return500();
            }
            if (!string.IsNullOrWhiteSpace(appUser.ProfilePhoto))
                appUser.ProfilePhoto = Path.Combine(_pathHelper.UserProfilePath, appUser.ProfilePhoto);
            return ServiceResponse<UserDto>.ReturnResultWith200(_mapper.Map<UserDto>(appUser));
        }
    }

}