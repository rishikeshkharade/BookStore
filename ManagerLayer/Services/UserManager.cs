using System;
using System.Collections.Generic;
using System.Text;
using CommonLayer.Models;
using ManagerLayer.Interfaces;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;

namespace ManagerLayer.Services
{
    public class UserManager : IUserManager
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        public UserManager(IUserRepository userRepository, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        public UserEntity Register(UserRegisterModel model)
        {
            return _userRepository.Register(model);
        }
        public UserEntity Login(UserLoginModel model)
        {
            return _userRepository.Login(model);
        }

        public ForgetPasswordModel ForgetPassword(string email)
        {
            var user = _userRepository.GetUserByEmail(email);
            if (user == null)
            {
                return null;
            }
            var token = _tokenService.GenerateToken(user.UserId, user.Email, user.Role);
            return new ForgetPasswordModel
            {
                Email = user.Email,
                Token = token
            };
        }

        public bool EmailChecker(string email)
        {
            return _userRepository.EmailChecker(email);
        }
    }
}
