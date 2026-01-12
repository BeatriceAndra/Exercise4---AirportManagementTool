using AirportTool.Application.DTOs.Auth;
using AirportTool.Application.Exceptions;
using AirportTool.Application.Interfaces.Service;
using AirportTool.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace AirportTool.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApiUser> _userManager;
        private readonly RoleManager<Microsoft.AspNetCore.Identity.IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<ApiUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
        {
            var userExist = await _userManager.FindByNameAsync(dto.UserName);
            if (userExist != null)
                throw new BadRequestException("Username already exists");

            var user = new ApiUser
            {
                UserName = dto.UserName,
                Email = dto.Email
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
                throw new BadRequestException(string.Join(", ", result.Errors.Select(e => e.Description)));

            if (!await _roleManager.RoleExistsAsync(dto.Role))
            {
                await _roleManager.CreateAsync(new Microsoft.AspNetCore.Identity.IdentityRole(dto.Role));
            }
            await _userManager.AddToRoleAsync(user, dto.Role);

            var token = await GenerateJwtTokenAsync(user);

            var roles = await _userManager.GetRolesAsync(user);

            return new AuthResponseDto
            {
                UserName = user.UserName,
                Email = user.Email,
                Token = token,
                Roles = roles
            };
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            var user = await _userManager.FindByNameAsync(dto.UserName);
            if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
                throw new BadRequestException("Invalid credentials");

            var token = await GenerateJwtTokenAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            return new AuthResponseDto
            {
                UserName = user.UserName,
                Email = user.Email,
                Token = token,
                Roles = roles
            };
        }

        private async Task<string> GenerateJwtTokenAsync(ApiUser user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(3),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
