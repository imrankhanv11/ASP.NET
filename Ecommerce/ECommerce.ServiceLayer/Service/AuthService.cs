using ECommerce.DataAccessLayer.Inteface;
using ECommerce.DataAccessLayer.Models;
using ECommerce.ServiceLayer.DTO.Authentications.Request;
using ECommerce.ServiceLayer.DTO.Authentications.Response;
using ECommerce.ServiceLayer.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.ServiceLayer.Service
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepo _repo;
        private readonly IConfiguration _config;

        public AuthService(IAuthRepo repo, IConfiguration config)
        {
            _repo = repo;
            _config = config;
        }

        public async Task<bool> RegisterUserService(RegisterUserDTO newUser)
        {
            var CheckExits = await _repo.CheckUserExitsRepo(newUser.Email);

            if(CheckExits != null)
            {
                throw new ValidationException("Email Already Exits");
            }

            var Password = BCrypt.Net.BCrypt.HashPassword(newUser.PasswordHash);

            var newOne = new User
            {
                Username = newUser.Username,
                PasswordHash = Password,
                Email = newUser.Email,
                RoleId = 3
            };

            var value = await _repo.RegisterUserRepo(newOne);

            return value;
        }

        public async Task<string> GenereteJWTTokenService(int id, string RoleName, bool IsAccessToken)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWTConnection:Key"]));
            var creads = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var AccessToken = int.Parse(_config["JWTConnection:AccessTokenExpire"]);
            var RefreshToken = int.Parse(_config["JWTConnection:RefreshToken"]);

            var Claim = new[]
            {
                new Claim(ClaimTypes.Role, RoleName),
                new Claim(ClaimTypes.NameIdentifier, id.ToString())
            };

            var token = new JwtSecurityToken(
                issuer : _config["JWTConnection:Issuer"],
                audience : _config["JWTConnection:Audience"],
                claims : Claim,
                signingCredentials : creads,
                expires : IsAccessToken ? DateTime.UtcNow.AddMinutes(AccessToken) : DateTime.UtcNow.AddDays(RefreshToken)
                );

            var JWTToken = new JwtSecurityTokenHandler().WriteToken(token);

            return JWTToken;
        }

        public async Task<LoginResponseDTO> LoginService(LoginRequestDTO dto)
        {
            var value = await _repo.CheckUserExitsRepo(dto.Email);

            if(value == null)
            {
                throw new ValidationException("User Name OR Password Wrong");
            }

            var password = BCrypt.Net.BCrypt.Verify(dto.Password, value.PasswordHash);

            if (!password)
            {
                throw new ValidationException("User Name or Password Wrong");
            }

            var AccesToken = await GenereteJWTTokenService(value.UserId, value.Role.RoleName, true);

            var RefreshToken = await GenereteJWTTokenService(value.UserId, value.Role.RoleName, false);

            var outputtoken = new LoginResponseDTO
            {
                AccessToken = AccesToken,
                RefreshToken = RefreshToken
            };

            return outputtoken;
        }

        public async Task<LoginResponseDTO> RefreshTokenService(string token)
        {
            try
            {
                var key = Encoding.UTF8.GetBytes(_config["JWTConnection:Key"]);

                var value = new JwtSecurityTokenHandler().ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _config["JWTConnection:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = _config["JWTConnection:Audience"],
                    ValidateLifetime = true, 
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);


                var userID = value.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
                var role = value.Claims.First(x => x.Type == ClaimTypes.Role).Value;

                var id = int.Parse(userID);

                var Accesstoken = await GenereteJWTTokenService(id, role, true);

                var Refreshtoken = await GenereteJWTTokenService(id, role, false);

                return new LoginResponseDTO
                {
                    AccessToken = Accesstoken,
                    RefreshToken = Refreshtoken
                };
            }
            catch
            {
                throw new UnauthorizedAccessException("Refresh token expired try to login");
            }
        }
    }
}
