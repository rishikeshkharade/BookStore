using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RepositoryLayer.Helpers;
using RepositoryLayer.Models;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;

namespace RepositoryLayer.Services
{
    public class AdminRepository : IAdminRepository
    {
        private readonly BookStoreDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly TokenService _tokenService;
        public AdminRepository(BookStoreDbContext dbContext, IConfiguration configuration, TokenService tokenService)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _tokenService = tokenService;
        }
        public AdminEntity Register(AdminRegisterModel model)
        {
            var admin = new AdminEntity
            {
                Email = model.Email,
                Password = PasswordHelper.HashPassword(model.Password),
                Role = "Admin"
            };

            _dbContext.Admins.Add(admin);
            _dbContext.SaveChanges();
            return admin;
        }
        public TokenModel Login(AdminLoginModel model)
        {
            var admin = _dbContext.Admins.FirstOrDefault(x => x.Email == model.Email);

            if (admin != null && PasswordHelper.VerifyPassword(model.Password, admin.Password))
            {
                return _tokenService.GenerateTokens(admin.AdminId, admin.Email, admin.Role);
            }

            return null;
        }
        //public bool EmailChecker(string email)
        //{
        //    return _dbContext.Admins.Any(x => x.Email == email);
        //}

        public string ForgetPassword(string email)
        {
            var admin = _dbContext.Admins.FirstOrDefault(a => a.Email == email);
            if (admin == null)
                return null;

            return _tokenService.GenerateResetToken(admin.Email, admin.Role);
        }

        public bool ResetPassword(ResetPasswordModel model)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

                var principal = tokenHandler.ValidateToken(model.Token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidAudience = _configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var email = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

                var admin = _dbContext.Admins.FirstOrDefault(a => a.Email == email);
                if (admin == null)
                    return false;

                if (PasswordHelper.VerifyPassword(model.NewPassword, admin.Password))
                    return false;

                admin.Password = PasswordHelper.HashPassword(model.NewPassword);
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
            var admin = _dbContext.Admins.FirstOrDefault(a =>
                a.RefreshToken == refreshToken &&
                a.RefreshTokenExpiryTime > DateTime.Now);

            if (admin == null) return null;

            var tokenModel = _tokenService.GenerateTokens(admin.AdminId, admin.Email, admin.Role);

            // Optionally rotate refresh token
            admin.RefreshToken = tokenModel.RefreshToken;
            admin.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);

            _dbContext.SaveChanges();

            return tokenModel;
        }


        //public AdminEntity GetAdminByEmail(string email)
        //{
        //    return _dbContext.Admins.FirstOrDefault(a => a.Email == email);
        //}
    }
}
