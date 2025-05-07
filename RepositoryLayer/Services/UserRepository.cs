using System;
using System.Linq;
using System.Text;
using RepositoryLayer.Helpers;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace RepositoryLayer.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly BookStoreDbContext _dbContext;
        private readonly TokenService _tokenService;
        private readonly IConfiguration _configuration;

        public UserRepository(BookStoreDbContext dbContext, TokenService tokenService, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _tokenService = tokenService;
            _configuration = configuration;
        }
        public UserEntity Register(UserRegisterModel model)
        {
            var user = new UserEntity
            {
                FullName = model.FullName,
                Email = model.Email,
                Password = PasswordHelper.HashPassword(model.Password),
                PhoneNumber = model.PhoneNumber,
                Role = "User"
            };
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
            return user;
        }

        public TokenModel Login(UserLoginModel model)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Email == model.Email);
            if (user != null && PasswordHelper.VerifyPassword(model.Password, user.Password))
            {
                // Generate new access and refresh token
                var tokenModel = _tokenService.GenerateTokens(user.UserId, user.Email, user.Role);

                // Store the refresh token in DB
                user.RefreshToken = tokenModel.RefreshToken;
                user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7); // or preferred duration

                _dbContext.SaveChanges();

                return tokenModel;
            }
            return null;
        }

        //public UserEntity GetUserByEmail(string email)
        //{
        //    return _dbContext.Users.FirstOrDefault(u => u.Email == email);
        //}

        public string ForgetPassword(string email)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Email == email);
            if (user == null)
                return null;

            return _tokenService.GenerateResetToken(user.Email, user.Role);
        }


        public bool ResetPassword(ResetPasswordModel model)
        {
            try
            {
                var principal = new JwtSecurityTokenHandler().ValidateToken(
                    model.Token,
                    new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidIssuer = _configuration["Jwt:Issuer"],
                        ValidAudience = _configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])),
                        ClockSkew = TimeSpan.Zero
                    },
                    out SecurityToken validatedToken
                );

                var email = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

                var user = _dbContext.Users.FirstOrDefault(u => u.Email == email);
                if (user == null)
                    return false;

                if (PasswordHelper.VerifyPassword(model.NewPassword, user.Password))
                    return false;

                user.Password = PasswordHelper.HashPassword(model.NewPassword);
                _dbContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public TokenModel Refresh(string refreshToken)
        {
            var user = _dbContext.Users.FirstOrDefault(u =>
                u.RefreshToken == refreshToken &&
                u.RefreshTokenExpiryTime > DateTime.Now);

            if (user == null) return null;

            var tokenModel = _tokenService.GenerateTokens(user.UserId, user.Email, user.Role);

            // Optionally replace old refresh token
            user.RefreshToken = tokenModel.RefreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
            _dbContext.SaveChanges();

            return tokenModel;
        }
    }
}
