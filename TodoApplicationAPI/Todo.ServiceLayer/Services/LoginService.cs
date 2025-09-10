using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Todo.DataAccessLayer.Interface;
using Todo.ModelLayer.DTO.Request;
using Todo.ModelLayer.DTO.Response;
using Todo.ModelLayer.Models;
using Todo.ServiceLayer.Interface;

namespace Todo.ServiceLayer.Services
{
    public class LoginService : ILoginService
    {
        private readonly ILoginRepo _rep;
        private readonly IConfiguration configuration;

        public LoginService(ILoginRepo repo, IConfiguration _config)
        {
            _rep = repo;
            configuration = _config;
        }

        public async Task AddRefresh(string token, string email, string roll)
        {
            var refreshTokenExpiryDays = int.Parse(configuration["JWTConfiguration:RefreshTokenExpiryDays"]);

            var value = new RefershToken
            {
                Token = token,
                UserEmail = email,
                Roll = roll,
                Expiration = DateTime.UtcNow.AddDays(refreshTokenExpiryDays),
                IsRevoked = false
            };
            await _rep.AddRefeshRepo(value);

        }

        public async Task<LoginAuthenticationDTO> CheckPassWordUserService(LoginDTO dto)
        {
            var value =await _rep.CheckPasswordUser(dto);

            var loginRoll = new LoginAuthenticationDTO
            {
                Roll = value.Roll,
                Email = value.Email
            };

            return loginRoll;
        }

        public async Task<LoginResponseDTO> GenerateJwtToken(string Email, string role)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTConfiguration:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var accessTokenExpiryMinutes = int.Parse(configuration["JWTConfiguration:AccessTokenExpiryMinutes"]);
            



            var claims = new[]
            {
                new Claim(ClaimTypes.Email, Email),
                new Claim(ClaimTypes.Role, role)
            };
             
            var token = new JwtSecurityToken(
                issuer: configuration["JWTConfiguration:Issuer"],
                audience: configuration["JWTConfiguration:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(accessTokenExpiryMinutes),
                signingCredentials: creds
            );

            var JWTtoken = new JwtSecurityTokenHandler().WriteToken(token);

            var RefershToken = GenerateRefreshToken();

            await AddRefresh(RefershToken, Email, role);

            return new LoginResponseDTO
            {
                Token = JWTtoken,
                Username = Email,
                RefreshToken = RefershToken,
                Expiration = DateTime.UtcNow.AddMinutes(accessTokenExpiryMinutes)
            };
        }

        public async Task<LoginResponseDTO> GenerateNewToken(string token)
        {
            var value = await _rep.CheckRefreshToken(token);

            if(value == null || value.Expiration < DateTime.UtcNow || value.IsRevoked)
            {
                throw new ArgumentException("Please Login Again");
            }

            await _rep.UpdateRevokeRefreshToken(value);
            
            var newtoken = await GenerateJwtToken(value.UserEmail, value.Roll);

            return newtoken;
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

    }
}
