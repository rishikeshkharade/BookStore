using System;
using System.Collections.Generic;
using System.Text;
using CommonLayer.Models;
using RepositoryLayer.Entity;

namespace RepositoryLayer.Interfaces
{
    public interface IUserRepository
    {
        UserEntity Register(UserRegisterModel model);
        UserEntity Login(UserLoginModel model);
        //ForgetPasswordModel ForgetPassword(string email);
        UserEntity GetUserByEmail(string email);
        bool EmailChecker(string email);
    }
}
