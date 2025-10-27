using LibararyManagement.Data.Interface;
using LibararyManagement.Data.Models;
using LibraryManagement.Service.DTO.Account.Request;
using LibraryManagement.Service.DTO.Account.Response;
using LibraryManagement.Service.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Service.Service
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepo _repo;
        private readonly IConfiguration _config;

        public AccountService(IAccountRepo repo, IConfiguration config)
        {
            _repo = repo;
            _config = config;
        }

        public enum Role
        {
            Admin = 1, User = 2
        }

        public async Task<UserRegisterResponseDTO> UserRegisterService(UserRegisterDTO dto)
        {
            if(dto.Name.Length <= 2)
            {
                throw new ValidationException("Name can't less than 2 Letters");
            }

            var user = await _repo.ExtistingUserCheckRepo(dto.Email);

            if(user != null)
            {
                throw new ValidationException("Email id already exits");
            }

            var hasshedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var newUser = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                Password = hasshedPassword,
                RoleId = (int)Role.User
            };

            var userAdded = await _repo.AddNewUserRepo(newUser);

            return new UserRegisterResponseDTO
            {
                Id = userAdded.Id,
                Email = userAdded.Email,
                Role = userAdded.Role.RoleName,
                Name = userAdded.Name
            };
        }

        public async Task<UserLoginResponstDTO> UserLoginService(UserLoginRequestDTO dto)
        {
            var user = await _repo.ExtistingUserCheckRepo(dto.Email);

            if(user == null)
            {
                throw new ValidationException("Email or Password Wrong");
            }

            var password = BCrypt.Net.BCrypt.Verify(dto.Password, user.Password);

            if (!password)
            {
                throw new ValidationException("Email or Password Wrong");
            }

            var Accesstoken = GenerateJWTToken(user.Id, user.Role.RoleName, true);
            var RefreshToken = GenerateJWTToken(user.Id, user.Role.RoleName, false);

            return new UserLoginResponstDTO
            {
                AccessToken = Accesstoken,
                RefreshToken = RefreshToken,
            };

        }

        public string GenerateJWTToken(int userId, string role, bool isAccessToken)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWTConnection:Key"]));
            var creads = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var Claim = new[]
            {

                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Role, role),
                new Claim("token_type", isAccessToken ? "access" : "refresh")

            };

            var AccessToken = int.Parse(_config["JWTConnection:AccessTokenExpire"]);
            var RefreshToken = int.Parse(_config["JWTConnection:RefreshToken"]);

            var token = new JwtSecurityToken(
                issuer: _config["JWTConnection:Issuer"],
                audience: _config["JWTConnection:Audience"],
                claims: Claim,
                signingCredentials: creads,
                expires: isAccessToken ? DateTime.UtcNow.AddMinutes(AccessToken) : DateTime.UtcNow.AddDays(RefreshToken)
                );

            var JWTToken = new JwtSecurityTokenHandler().WriteToken(token);

            return JWTToken;
        }

        public async Task<UserLoginResponstDTO> RefreshTokenService(string refreshToken)
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

                var Accesstoken =  GenerateJWTToken(id, role, true);

                var Refreshtoken =  GenerateJWTToken(id, role, false);

                return new UserLoginResponstDTO
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
    }
}
