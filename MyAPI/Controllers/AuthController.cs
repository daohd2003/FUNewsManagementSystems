using AutoMapper;
using Configurations;
using FUDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ISystemAccountRepository _repo;
        private readonly IMapper _mapper;
        private readonly JwtSettings _jwtSettings;
        private readonly AdminAccountOptions _adminOptions;

        public AuthController(ISystemAccountRepository repo, IMapper mapper,
            IOptions<JwtSettings> jwtOptions, IOptions<AdminAccountOptions> adminOptions)
        {
            _repo = repo;
            _mapper = mapper;
            _jwtSettings = jwtOptions.Value;
            _adminOptions = adminOptions.Value;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginDto)
        {
            BusinessObjects.SystemAccount? user = await _repo.GetByEmail(loginDto.Email);
            string roleName = "";

            if (user == null)
            {
                // Tài khoản mặc định Admin từ appsettings.json
                if (loginDto.Email == _adminOptions.Email && loginDto.Password == _adminOptions.Password)
                {
                    user = new BusinessObjects.SystemAccount
                    {
                        AccountEmail = _adminOptions.Email,
                        AccountName = "System Admin",
                        AccountRole = 0
                    };
                    roleName = "Admin";
                }
                else
                {
                    return Unauthorized("Invalid credentials");
                }
            }
            else
            {
                // Có user trong DB, kiểm tra password
                if (user.AccountPassword != loginDto.Password)
                {
                    return Unauthorized("Invalid password");
                }

                roleName = user.AccountRole switch
                {
                    1 => "Staff",
                    2 => "Lecturer",
                    _ => "Unknown"
                };
            }

            var token = GenerateToken(user, roleName);

            var response = new LoginResponseDTO
            {
                Token = token
            };

            return Ok(response);
        }

        private string GenerateToken(BusinessObjects.SystemAccount user, string roleName)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.AccountId.ToString()),
                new Claim(ClaimTypes.Email, user.AccountEmail),
                new Claim(ClaimTypes.Role, roleName),
                new Claim(ClaimTypes.Name, user.AccountName),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
