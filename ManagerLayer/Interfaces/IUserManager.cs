using System;
using System.Collections.Generic;
using System.Text;
using RepositoryLayer.Entity;
using RepositoryLayer.Models;

namespace ManagerLayer.Interfaces
{
    public interface IUserManager
    {
        UserEntity Register(UserRegisterModel model);
        string Login(UserLoginModel model);
        ForgetPasswordModel ForgetPassword(string email);
        bool EmailChecker(string email);
        bool ResetPassword(ResetPasswordModel model);
    }
}
