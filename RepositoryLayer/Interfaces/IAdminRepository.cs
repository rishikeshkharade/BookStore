using System;
using System.Collections.Generic;
using System.Text;
using CommonLayer.Models;
using RepositoryLayer.Entity;

namespace RepositoryLayer.Interfaces
{
    public interface IAdminRepository
    {
        AdminEntity Register(AdminRegisterModel model);
        AdminEntity Login(AdminLoginModel model);
        bool EmailChecker(string email);
        bool ResetPassword(string email, string newHashedPassword);
        AdminEntity GetAdminByEmail(string email);
    }
}
