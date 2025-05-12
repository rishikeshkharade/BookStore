using System;
using System.Collections.Generic;
using System.Text;
using RepositoryLayer.Entity;
using RepositoryLayer.Models;

namespace ManagerLayer.Interfaces
{
    public interface IUserManager
    {
        UserResponseModel Register(UserRegisterModel model);
        UserLoginResponseModel Login(UserLoginModel model);
        public string ForgetPassword(string email);
        //bool EmailChecker(string email);
        bool ResetPassword(ResetPasswordModel model);
        TokenModel Refresh(string refreshToken);
    }
}
