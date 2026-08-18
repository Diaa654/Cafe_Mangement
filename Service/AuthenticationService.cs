using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
        , IConfiguration _configuration) : IAuthenticationService
    {
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
