using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RepositoryLayer.Helpers;
using RepositoryLayer.Models;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;

namespace RepositoryLayer.Services
{
    public class AdminRepository : IAdminRepository
    {
        private readonly BookStoreDbContext _dbContext;
        public AdminRepository(BookStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public AdminEntity Register(AdminRegisterModel model)
        {
            var admin = new AdminEntity
            {
                Email = model.Email,
                Password = PasswordHelper.HashPassword(model.Password),
                Role = "Admin"
            };

            _dbContext.Admins.Add(admin);
            _dbContext.SaveChanges();
            return admin;
        }
        public AdminEntity Login(AdminLoginModel model)
        {
            var admin = _dbContext.Admins.FirstOrDefault(x => x.Email == model.Email);

            if (admin != null && PasswordHelper.VerifyPassword(model.Password, admin.Password))
            {
                return admin;
            }

            return null;
        }
        public bool EmailChecker(string email)
        {
            return _dbContext.Admins.Any(x => x.Email == email);
        }
        public bool ResetPassword(string email, string newHashedPassword)
        {
            var admin = _dbContext.Admins.FirstOrDefault(x => x.Email == email);
            if (admin == null) return false;

            admin.Password = newHashedPassword;
            _dbContext.SaveChanges();
            return true;
        }
        public AdminEntity GetAdminByEmail(string email)
        {
            return _dbContext.Admins.FirstOrDefault(a => a.Email == email);
        }
    }
}
