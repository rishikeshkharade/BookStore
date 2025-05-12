using System;
using System.Collections.Generic;
using System.Text;
using RepositoryLayer.Models;
using RepositoryLayer.Entity;

namespace RepositoryLayer.Interfaces
{
    public interface IAdminRepository
    {
        AdminEntity Register(AdminRegisterModel model);
        AdminLoginResponseModel Login(AdminLoginModel model);
        //bool EmailChecker(string email);
        public string ForgetPassword(string email);
        bool ResetPassword(ResetPasswordModel resetPasswordModel);
        //AdminEntity GetAdminByEmail(string email);
        public TokenModel Refresh(string refreshToken);
    }
}
