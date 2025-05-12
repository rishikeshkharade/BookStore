using System;
using System.Collections.Generic;
using System.Text;
using RepositoryLayer.Entity;
using RepositoryLayer.Models;

namespace ManagerLayer.Interfaces
{
    public interface IAdminManager
    {
        AdminEntity Register(AdminRegisterModel model);
        AdminLoginResponseModel Login(AdminLoginModel model);
        string ForgetPassword(string email);
        //bool EmailChecker(string email);
        bool ResetPassword(ResetPasswordModel model);
        TokenModel Refresh(string refreshToken);
    }
}
