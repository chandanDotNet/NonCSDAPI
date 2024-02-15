using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using POS.Data.Dto;
using POS.Helper;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using POS.Data;
using Microsoft.EntityFrameworkCore;

namespace POS.MediatR.Handlers
{
    public class AppUserLoginCommandHandler : IRequestHandler<AppUserLoginCommand, ServiceResponse<UserAuthDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly ILoginAuditRepository _loginAuditRepository;
        private readonly IHubContext<UserHub, IHubClient> _hubContext;
        private readonly PathHelper _pathHelper;

        public AppUserLoginCommandHandler(
            IUserRepository userRepository,
            SignInManager<User> signInManager,
            UserManager<User> userManager,
            ILoginAuditRepository loginAuditRepository,
            IHubContext<UserHub, IHubClient> hubContext,
            PathHelper pathHelper
            )
        {
            _userRepository = userRepository;
            _signInManager = signInManager;
            _userManager = userManager;
            _loginAuditRepository = loginAuditRepository;
            _hubContext = hubContext;
            _pathHelper = pathHelper;
        }
        public async Task<ServiceResponse<UserAuthDto>> Handle(AppUserLoginCommand request, CancellationToken cancellationToken)
        {
            //var loginAudit = new LoginAuditDto
            //{
            //    UserName = request.UserName,
            //    RemoteIP = request.RemoteIp,
            //    Status = LoginStatus.Error.ToString(),
            //    Latitude = request.Latitude,
            //    Longitude = request.Longitude,
            //};

            //var user = await _userManager.FindByNameAsync(request.UserName);

            //if (user == null)
            //{
            //    await _loginAuditRepository.LoginAudit(loginAudit);
            //    return ServiceResponse<UserAuthDto>.ReturnFailed(401, "UserName Or Password is InCorrect.");
            //}

            //var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

            //if (result.Succeeded)
            //{
            var userInfo = await _userRepository
                .All.Include(x => x.Counter)
                .Where(c => c.PhoneNumber == request.MobileNumber && c.Otp == request.otp)
                .FirstOrDefaultAsync();
            if (!userInfo.IsActive)
            {
                //await _loginAuditRepository.LoginAudit(loginAudit);
                return ServiceResponse<UserAuthDto>.ReturnFailed(401, "UserName Or Password is InCorrect.");
            }

            //loginAudit.Status = LoginStatus.Success.ToString();
            //await _loginAuditRepository.LoginAudit(loginAudit);
            var authUser = await _userRepository.BuildUserAuthObject(userInfo);
            var onlineUser = new SignlarUser
            {
                Email = authUser.Email,
                Id = authUser.Id.ToString()
            };
            await _hubContext.Clients.All.Joined(onlineUser);

            if (!string.IsNullOrWhiteSpace(authUser.ProfilePhoto))
            {
                authUser.ProfilePhoto = Path.Combine(_pathHelper.UserProfilePath, authUser.ProfilePhoto);
            }
            if (userInfo != null)
            {
                return ServiceResponse<UserAuthDto>.ReturnResultWith200(authUser);
            }

            else
            {
                //await _loginAuditRepository.LoginAudit(loginAudit);
                return ServiceResponse<UserAuthDto>.ReturnFailed(401, "UserName Or Password is InCorrect.");
            }
        }
    }
}