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
        public TokenModel Login(AdminLoginModel model)
        {
            return _adminRepository.Login(model);
        }
        public string ForgetPassword(string email)
        {
            return _adminRepository.ForgetPassword(email);
        }
        //public bool EmailChecker(string email)
        //{
        //    return _adminRepository.EmailChecker(email);
        //}
        public bool ResetPassword(ResetPasswordModel model)
        {
            return _adminRepository.ResetPassword(model);
        }

        public TokenModel Refresh(string refreshToken)
        {
            return _adminRepository.Refresh(refreshToken);
        }
    }
}
