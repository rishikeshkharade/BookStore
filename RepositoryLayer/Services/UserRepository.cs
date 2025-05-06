using System;
using System.Collections.Generic;
using System.Text;
using CommonLayer.Helpers;
using CommonLayer.Models;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;

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

    }
}
