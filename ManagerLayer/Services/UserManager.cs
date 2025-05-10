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
    public class UserManager : IUserManager
    {
        private readonly IUserRepository _userRepository;
        
        public UserManager(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public UserResponseModel Register(UserRegisterModel model)
        {
            return _userRepository.Register(model);
        }
        public TokenModel Login(UserLoginModel model)
        {
            return _userRepository.Login(model);
        }

        public string ForgetPassword(string email)
        {
            return _userRepository.ForgetPassword(email);
        }

        //public bool EmailChecker(string email)
        //{
        //    return _userRepository.EmailChecker(email);
        //}

        public bool ResetPassword(ResetPasswordModel model)
        {
            return _userRepository.ResetPassword(model);
        }

        public TokenModel Refresh(string refreshToken)
        {
            return _userRepository.Refresh(refreshToken);
        }
    }
}
