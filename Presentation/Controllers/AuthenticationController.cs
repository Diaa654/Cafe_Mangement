using Microsoft.AspNetCore.Mvc;
using ServiceAbstraction;
using Shared.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    public class AuthenticationController(IAuthenticationService _authenticationService) : ApiBaseController
    {
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto login)
        {
            var Result = await _authenticationService.LoginAsync(login);
            return HandleResult(Result);
        }
        [HttpPost("SaveDeviceToken")]
        public async Task<ActionResult<string>> SaveDeviceToken(SaveDeviceTokenDto saveDeviceTokenDto)
        {
            int userId=GetUserId();
            saveDeviceTokenDto.UserId = userId;
            var Result = await _authenticationService.SaveDeviceTokenAsync(saveDeviceTokenDto);
            return HandleResult(Result);
        }
    }
}