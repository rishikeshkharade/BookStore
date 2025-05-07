using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using RepositoryLayer.Helpers;
using ManagerLayer.Interfaces;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Models;
using RepositoryLayer.Services;

namespace ManagerLayer.Services
{
    public class AdminManager : IAdminManager
    {
        private readonly IAdminRepository _adminRepository;
        private readonly TokenService _tokenService;
        public AdminManager(IAdminRepository adminRepository, TokenService tokenService)
        {
            _adminRepository = adminRepository;
            _tokenService = tokenService;
        }
        public AdminEntity Register(AdminRegisterModel model)
        {
            return _adminRepository.Register(model);
        }
        public string Login(AdminLoginModel model)
        {
            var admin = _adminRepository.Login(model);
            if (admin != null)
            {
                return _tokenService.GenerateToken(admin.AdminId, admin.Email, admin.Role);
            }
            return null;
        }
        public ForgetPasswordModel ForgetPassword(string email)
        {
            var admin = _adminRepository.GetAdminByEmail(email);
            if (admin == null) return null;

            var token = _tokenService.GenerateToken(admin.AdminId, admin.Email, admin.Role);

            return new ForgetPasswordModel
            {
                Email = admin.Email,
                Token = token
            };
        }
        public bool EmailChecker(string email)
        {
            return _adminRepository.EmailChecker(email);
        }
        public bool ResetPassword(ResetPasswordModel model)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.ReadJwtToken(model.Token);
            var tokenEmail = token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            if (tokenEmail != model.Email)
                return false;

            var admin = _adminRepository.GetAdminByEmail(model.Email);
            if (admin == null)
                return false;

            if (PasswordHelper.VerifyPassword(model.NewPassword, admin.Password))
                return false;

            var hashedPassword = PasswordHelper.HashPassword(model.NewPassword);
            return _adminRepository.ResetPassword(model.Email, hashedPassword);
        }
    }
}
