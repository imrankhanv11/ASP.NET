using ECommerce.ServiceLayer.DTO.Authentications.Request;
using ECommerce.ServiceLayer.DTO.Authentications.Response;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using ShopSphere.DataAccessLayer.Interface;
using ShopSphere.DataAccessLayer.Models;
using ShopSphere.ServiceLayer.DTO.Authentication.Request;
using ShopSphere.ServiceLayer.DTO.Authentication.Response;
using ShopSphere.ServiceLayer.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ShopSphere.ServiceLayer.Services
{
    public class AuthService : IAuthService
    {

        private readonly IAthenticationRepo _repo;
        private readonly ILogger<AuthService> _logger;
        private readonly IConfiguration _config;

        public AuthService(IAthenticationRepo repo, ILogger<AuthService> logger, IConfiguration config)
        {
            _logger = logger;
            _repo = repo;
            _config = config;
        }

        public async Task<int> RegisterUserService(RegisterUserDTO newUser)
        {
            var CheckExits = await _repo.CheckUserExitsRepo(newUser.Email);

            // Checking User Alredy have Account
            if (CheckExits != null)
            {
                _logger.LogError("User Already Have Account");
                throw new ValidationException("Email Already Exits");
            }

            // Name Validations
            if(newUser.Name.Trim().Length <= 3)
            {
                throw new ValidationException("Name atleast need more than 3 Char");
            }

            // Password Hashing
            var Password = BCrypt.Net.BCrypt.HashPassword(newUser.PasswordHash);
            _logger.LogInformation("Password Hashed");

            var newOne = new User
            {
                Name = newUser.Name,
                PasswordHash = Password,
                Email = newUser.Email,
                RoleId = 2, // User ID
                CreatedAt = DateOnly.FromDateTime((DateTime.Now))
            };

            var value = await _repo.RegisterUserRepo(newOne);
            _logger.LogInformation("Register Succesfully");

            return newOne.UserId;
        }

        public async Task<LoginResponseDTO> LoginService(LoginRequestDTO dto)
        {
            var value = await _repo.CheckUserExitsRepo(dto.Email);

            if (value == null)
            {
                _logger.LogError("User Not have Account");
                throw new ValidationException("User Name or Password Miss Match");
            }

            var password = BCrypt.Net.BCrypt.Verify(dto.Password, value.PasswordHash);

            if (!password)
            {
                _logger.LogError("password Mis match");
                throw new ValidationException("User Name or Password Miss Match");
            }

            var AccesToken = await GenereteJWTTokenService(value.UserId, value.Role.RoleName, true);

            var RefreshToken = await GenereteJWTTokenService(value.UserId, value.Role.RoleName, false);

            _logger.LogInformation("Token generted");

            var outputtoken = new LoginResponseDTO
            {
                AccessToken = AccesToken,
                RefreshToken = RefreshToken,
                User = new LoginUserResponseDTO
                {
                    UserId = value.UserId,
                    Name = value.Name,
                    Email = value.Email,
                    Role = value.Role.RoleName,
                    CreatedAt = value.CreatedAt
                }
            };

            return outputtoken;
        }

        public async Task<string> GenereteJWTTokenService(int id, string RoleName, bool AccessTokenValid)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWTConnection:Key"]));
            var creads = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var Claim = new[]
            {

                new Claim(ClaimTypes.NameIdentifier, id.ToString()),
                new Claim(ClaimTypes.Role, RoleName),
                new Claim("role", RoleName)

            };

            var AccessToken = int.Parse(_config["JWTConnection:AccessTokenExpire"]);
            var RefreshToken = int.Parse(_config["JWTConnection:RefreshToken"]);

            var token = new JwtSecurityToken(
                issuer: _config["JWTConnection:Issuer"],
                audience: _config["JWTConnection:Audience"],
                claims: Claim,
                signingCredentials: creads,
                expires: AccessTokenValid ? DateTime.UtcNow.AddMinutes(AccessToken) : DateTime.UtcNow.AddDays(RefreshToken)
                );

            var JWTToken = new JwtSecurityTokenHandler().WriteToken(token);

            return JWTToken;
        }

        public async Task<LoginResponseDTO> RefreshTokenService(string refreshToken)
        {
            try
            {
                var key = Encoding.UTF8.GetBytes(_config["JWTConnection:Key"]);

                var CheckToken = new JwtSecurityTokenHandler().ValidateToken(refreshToken, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidAudience = _config["JWTConnection:Audience"],
                    ValidIssuer = _config["JWTConnection:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken newToken);


                var role = CheckToken.Claims.First(x => x.Type == ClaimTypes.Role).Value;
                var userID = CheckToken.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;

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
                throw new UnauthorizedAccessException("Refresh Token was expired. Please Login");
            }
        }

        //Practice
        public string GenerateToken(int id, string role, bool accestoke)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWTConnection: Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Role, role),
                new Claim(ClaimTypes.NameIdentifier, id.ToString())
            };

            var token = new JwtSecurityToken(
                   issuer: _config["JWTConnection: issuser"],
                   audience: _config["JwtConnection: audience"],
                   signingCredentials: creds,
                   claims: claims,
                   expires: accestoke ? DateTime.Now.AddMinutes(int.Parse(_config["JWTConnection: Accesstoken"])) : DateTime.Now.AddDays(int.Parse(_config["Jwtconnection: refresh"]))
                );

            var tokenvalue = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenvalue;
        }

        public void RefreshToken(string token)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWTConnection: Key"]));

            var newToken = new JwtSecurityTokenHandler().ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _config["JWTConnection : issuer"],
                ValidAudience = _config["JWTConnection: audience"],
                ClockSkew = TimeSpan.Zero,
                IssuerSigningKey = key
            }, out SecurityToken tokenValue);

            var role = newToken.Claims.First(s=> s.Type == ClaimTypes.Role).Value;
            var id = newToken.Claims.First(s => s.Type == ClaimTypes.NameIdentifier).Value;
        }
    }
}
