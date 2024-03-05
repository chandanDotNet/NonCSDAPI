using MediatR;
using Microsoft.AspNetCore.Mvc;
using POS.Data.Dto;
using POS.MediatR.CommandAndQuery;
using System.Threading.Tasks;
using System;
using POS.Data.Entities;

namespace POS.API.Controllers.MobileLogin
{
    [Route("api/[controller]")]
    [ApiController]
    public class MobileAppLoginController : BaseController
    {
        public IMediator _mediator;
        public MobileAppLoginController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// App User Login
        /// </summary>
        /// <param name="appUserProfileCommand"></param>
        /// <returns></returns>
        [HttpPost]
        [Produces("application/json", "application/xml", Type = typeof(UserAuthDto))]
        public async Task<IActionResult> AppLogin(UpdateAppUserProfileCommand appUserProfileCommand)
        {
            MobileAppLoginResponseData response = new MobileAppLoginResponseData();
            //userLoginCommand.RemoteIp = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            try
            {
                var result = await _mediator.Send(appUserProfileCommand);

                if (result != null)
                {
                    MobileAppLogin data = new MobileAppLogin()
                    {
                        Mobile = result.Data.PhoneNumber,
                        Otp = result.Data.Otp.Value,
                        FirstName = result.Data.FirstName,
                        LastName = result.Data.LastName,
                        RoleType = result.Data.Provider,
                    };
                    response.status = true;
                    response.StatusCode = 1;
                    response.message = "success";
                    response.Data = data;
                }
                else
                {
                    response.status = false;
                    response.StatusCode = 0;
                    response.message = "Login failed.";
                }
            }
            catch (Exception ex)
            {
                response.status = false;
                response.StatusCode = 0;
                response.message = ex.Message;
            }

            return Ok(response);
            //return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// App User Login Otp Verification
        /// </summary>
        /// <param name="appUserLoginCommand"></param>
        /// <returns></returns>
        [HttpPost("AppLoginOtpVerification")]
        [Produces("application/json", "application/xml", Type = typeof(UserAuthDto))]
        public async Task<IActionResult> AppLoginOtpVerification(AppUserLoginCommand appUserLoginCommand)
        {
            MobileAppLoginTokenResponseData response = new MobileAppLoginTokenResponseData();
            try
            {
                appUserLoginCommand.RemoteIp = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                var result = await _mediator.Send(appUserLoginCommand);
                if (result != null)
                {
                    MobileAppLoginToken tokenData = new MobileAppLoginToken()
                    {
                        Token = result.Data.BearerToken,
                        FirstName = result.Data.FirstName,
                        LastName= result.Data.LastName,
                    };
                    response.status = true;
                    response.StatusCode = 1;
                    response.message = "success";
                    response.Data = tokenData;
                }
                else
                {
                    response.status = false;
                    response.StatusCode = 0;
                    response.message = "Validation failed.";
                }
            }
            catch (Exception ex)
            {
                response.status = false;
                response.StatusCode = 0;
                response.message = ex.Message;
            }

            return Ok(response);
            //return ReturnFormattedResponse(result);
        }
    }
}