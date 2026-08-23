using Domain.Contracts;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using ServiceAbstraction;
using Shared;
using Shared.CommonResult;
using Shared.DTOS;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class AuthenticationService(UserManager<User> _userManager
        , IConfiguration _configuration,IServiceManger _serviceManger) : IAuthenticationService
    {
        public async Task<Result> ActiveUser(int id, bool isActive) 
        {
            
                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);

                if (user == null)
                {
                    return Error.NotFound("UserNotFound", "المستخدم غير موجود بالأنظمة.");
                }

                user.IsActive = isActive;
                var result = await _userManager.UpdateAsync(user);

                if (!result.Succeeded)
                {
                    return Error.Failure("UpdateFailed", "حدث خطأ أثناء تحديث الحالة.");
                }

                return Result.Ok();
           
        }

        public async Task<Result<IEnumerable<GetAllUserDTO>>> GetAllUserAsync() 
        {
           
                var users = await _userManager.Users
                    .Select(u => new GetAllUserDTO
                    {
                        FuName = u.FullName,
                        Email = u.Email!,
                        IsActive = u.IsActive
                    })
                    .ToListAsync();

                return Result<IEnumerable<GetAllUserDTO>>.Ok(users);
        }
        public async Task<Result<UserDto>> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.Users
                    .FirstOrDefaultAsync(u => u.Email == loginDto.Email);
            if (user == null)
            {
                return Error.InvalidCrendentials("Auth.Invalid", "البريد الإلكتروني أو كلمة المرور غير صحيحة.");
            }
            
            var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!isPasswordValid)
            {
                return Error.InvalidCrendentials("Auth.Invalid", "البريد الإلكتروني أو كلمة المرور غير صحيحة.");
            }
            var userRoles = (await _userManager.GetRolesAsync(user)).ToList();
            return new UserDto
            {
                FullName = $"{user.FullName}!",
                Email = user.Email!,
                Roles = userRoles,
                Token = await CreateTokenAsync(user)
            };
        }

        public async Task<Result<string>> SaveDeviceTokenAsync( SaveDeviceTokenDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.DeviceToken))
                return Error.Failure("InvalidToken", "التوكن غير صالح أو فارغ.");
            var user = await _userManager.FindByIdAsync(dto.UserId.ToString());
            if (user == null)
                return Error.Failure("UserNotFound", "المستخدم غير موجود بالأنظمة.");
            var result = await _userManager.SetAuthenticationTokenAsync(
                user,
                loginProvider: "FCM",
                tokenName: "DeviceToken",
                tokenValue: dto.DeviceToken
            );

            if (!result.Succeeded)
                return Error.Failure("UpdateFailed", "حدث خطأ أثناء حفظ الإعدادات.");
            var userRole=await _userManager.GetRolesAsync(user);
            if (userRole.Contains(AppRoles.Waiter.ToString()))
                await _serviceManger.FcmService.SubscribeWaiterToTopicAsync(dto.DeviceToken);
            if (userRole.Contains(AppRoles.Barista.ToString()))
                await _serviceManger.FcmService.SubscribeBaristaToTopicAsync(dto.DeviceToken);

            return Result<string>.Ok("تم حفظ التوكن بنجاح.");
        }

        private async Task<string> CreateTokenAsync(User user)
        {
            var Claims = new List<Claim>
               {

                new (ClaimTypes.Email,user.Email!),
                new (ClaimTypes.Name,user.UserName!),
                new (ClaimTypes.NameIdentifier,user.Id.ToString()),
               };
            var Roles = await _userManager.GetRolesAsync(user);
            foreach (var role in Roles)
            {
                Claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var SecretKey = _configuration.GetSection("JWTOptions")["SecretKey"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var Token = new JwtSecurityToken(
                issuer: _configuration["JWTOptions:Issuer"],
                audience: _configuration["JWTOptions:Audience"],
                claims: Claims,
                expires: DateTime.UtcNow.AddDays(30),
                signingCredentials: creds
                );
            return new JwtSecurityTokenHandler().WriteToken(Token);
        }



    }
}
