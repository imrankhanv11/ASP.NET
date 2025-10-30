using Azure.Core;
using BookManagement.DataAccessLayer.Interfaces;
using BookManagement.DataAccessLayer.Models;
using BookManagement.ServiceLayer.DTO.Login.Request;
using BookManagement.ServiceLayer.DTO.Login.Response;
using BookManagement.ServiceLayer.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BookManagement.ServiceLayer.Services
{
    public class LoginService : ILoginService
    {
        private readonly ILoginRepo _repo;

        private readonly IConfiguration _config;
        public LoginService(ILoginRepo repo, IConfiguration config)
        {
            _repo = repo;
            _config = config;
        }

        // REGISTER USER
        public async Task<bool> RegisterUserService(string userName, string password)
        {
            var checkUserexit = await _repo.CheckUserExitsRepo(userName);

            if (checkUserexit != null)
            {
                throw new ValidationException("UserName already exits");
            }

            var hashPassword = BCrypt.Net.BCrypt.HashPassword(password);

            var newUser = new User
            {
                Username = userName,
                PasswordHash = hashPassword,
                RoleId = 2
            };

            return await _repo.RegisterUserRepo(newUser);
        }

        // LOGIN USER
        public async Task<LoginResponseDTO> LoginUserService(LoginReqDTO userLogin)
        {
            var checkUserexit = await _repo.CheckUserExitsRepo(userLogin.UserName);

            if(checkUserexit == null)
            {
                throw new ValidationException("UserName or Password Wrong");
            }

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(userLogin.Password, checkUserexit.PasswordHash);

            if (!isPasswordValid)
            {
                throw new ValidationException("UserName or Password Wrong");
            }

            var Accesstoken = await GenerateJWTToken(checkUserexit.Role.RoleName, checkUserexit.UserId, true);

            var Refreshtoken = await GenerateJWTToken(checkUserexit.Role.RoleName, checkUserexit.UserId, false);

            return new LoginResponseDTO
            {
                AccessToken = Accesstoken,
                RefreshToken = Refreshtoken,
                User = new LoginResponseUserDTO
                {
                    Username = checkUserexit.Username,
                    UserId = checkUserexit.UserId,
                    Role = checkUserexit.Role.RoleName
                }
            };
        }

        // GENERATE TOKEN
        public async Task<string> GenerateJWTToken(string RoleName, int UserId, bool isAccestoken)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWTConnection:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var AccessToken = int.Parse(_config["JWTConnection:AccessTokenExpire"]);
            var RefreshToken = int.Parse(_config["JWTConnection:RefreshToken"]);

            var cliams = new[]
            {
                new Claim(ClaimTypes.Role, RoleName),
                new Claim(ClaimTypes.NameIdentifier, UserId.ToString())
            };

            var token = new JwtSecurityToken(
                issuer : _config["JWTConnection:Issuer"],
                audience : _config["JWTConnection:Audience"],
                claims : cliams,
                expires: isAccestoken
                        ? DateTime.UtcNow.AddMinutes(AccessToken)
                        : DateTime.UtcNow.AddDays(RefreshToken),
                signingCredentials: creds
                );

            var JWTToken = new JwtSecurityTokenHandler().WriteToken(token);

            return JWTToken;
        }

        // REFRESH TOKEN
        public async Task<LoginResponseDTO> RefreshTokenService(string token)
        {
            try
            {
                var key = Encoding.UTF8.GetBytes(_config["JWTConnection:Key"]);

                var principal = new JwtSecurityTokenHandler().ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _config["JWTConnection:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = _config["JWTConnection:Audience"],
                    ValidateLifetime = true, // false
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                //var jwtToken = (JwtSecurityToken)validatedToken;

                //if (jwtToken.ValidTo < DateTime.UtcNow)
                //    throw new SecurityTokenExpiredException("Refresh token expired.");

                var userID = principal.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
                var role = principal.Claims.First(x => x.Type == ClaimTypes.Role).Value;

                var id = int.Parse(userID);

                var Accesstoken = await GenerateJWTToken(role, id, true);

                var Refreshtoken = await GenerateJWTToken(role, id, false);

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
