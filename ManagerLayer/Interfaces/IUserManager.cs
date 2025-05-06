using System;
using System.Collections.Generic;
using System.Text;
using CommonLayer.Models;
using RepositoryLayer.Entity;

namespace ManagerLayer.Interfaces
{
    public interface IUserManager
    {
        UserEntity Register(UserRegisterModel model);
        UserEntity Login(UserLoginModel model);
        ForgetPasswordModel ForgetPassword(string email);
        bool EmailChecker(string email);
    }
}
