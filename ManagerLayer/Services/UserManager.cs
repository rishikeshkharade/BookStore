using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using CommonLayer.Helpers;
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
        public string Login(UserLoginModel model)
        {
            var user = _userRepository.Login(model);
            if (user != null)
            {
                return _tokenService.GenerateToken(user.UserId, user.Email, user.Role);
            }

            return null;

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

        public bool ResetPassword(ResetPasswordModel model)
        {
            // Validate token (decoding and comparing email)
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.ReadJwtToken(model.Token);
            var tokenEmail = token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            if (tokenEmail != model.Email)
                return false;

                var user = _userRepository.GetUserByEmail(model.Email);
                if (user == null)
                    return false;

                // Verifies if new password is same as old one
                bool isSame = PasswordHelper.VerifyPassword(model.NewPassword, user.Password);
                if (isSame)
                    return false;
            

            // Hash new password
            var hashedPassword = PasswordHelper.HashPassword(model.NewPassword);

            // Update via repository
            return _userRepository.ResetPassword(model.Email, hashedPassword);
        }
    }
}
