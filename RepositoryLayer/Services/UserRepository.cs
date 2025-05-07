using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonLayer.Helpers;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Models;

namespace RepositoryLayer.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly BookStoreDbContext _dbContext;
       
        public UserRepository(BookStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public UserEntity Register(UserRegisterModel model)
        {
            var user = new UserEntity
            {
                FullName = model.FullName,
                Email = model.Email,
                Password = PasswordHelper.HashPassword(model.Password),
                PhoneNumber = model.PhoneNumber,
                Role = "User"
            };
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
            return user;
        }

        public UserEntity Login(UserLoginModel model)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Email == model.Email);
            if (user != null && PasswordHelper.VerifyPassword(model.Password, user.Password))
            {
                return user;
            }
            return null;
        }

        public UserEntity GetUserByEmail(string email)
        {
            return _dbContext.Users.FirstOrDefault(u => u.Email == email);
        }
        //public ForgetPasswordModel ForgetPassword(string email)
        //{
        //    var user = GetUserByEmail(email);
        //    if (user == null)
        //    {
        //        return null;
        //    }
        //    var token = _tokenSerice.GenerateToken(user.UserId, user.Email, user.Role);

        //    return new ForgetPasswordModel
        //    {
        //        Email = user.Email,
        //        Token = token
        //    };
        //}

        public bool EmailChecker(string email)
        {
            return _dbContext.Users.Any(u => u.Email == email);
        }

        public bool ResetPassword(string email, string hashedNewPassword)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Email == email);
            if (user != null)
            {
                user.Password = hashedNewPassword;
                _dbContext.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
