using System;
using System.Collections.Generic;
using System.Text;
using RepositoryLayer.Entity;
using RepositoryLayer.Models;

namespace RepositoryLayer.Interfaces
{
    public interface IUserRepository
    {
        UserEntity Register(UserRegisterModel model);
        TokenModel Login(UserLoginModel model);
        public string ForgetPassword(string email);
        //UserEntity GetUserByEmail(string email);
        //bool EmailChecker(string email);
        bool ResetPassword(ResetPasswordModel resetPasswordModel);
        public TokenModel Refresh(string refreshToken);
    }
}
