using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Models;

namespace RepositoryLayer.Services
{
    public class CustomerDetailRepository : ICustomerDetailRepository
    {
        private readonly BookStoreDbContext _ctx;
        private readonly IHttpContextAccessor _http;

        public CustomerDetailRepository(
            BookStoreDbContext ctx,
            IHttpContextAccessor http)
        {
            _ctx = ctx;
            _http = http;
        }

        public (CustomerDetail Detail, bool WasCreated) Upsert(CustomerDetailRequestModel dto)
        {
            // get current userId from JWT
            var userId = int.Parse(
                _http.HttpContext.User
                    .FindFirst("Id")?.Value
                ?? throw new Exception("UserId claim missing"));

            // looking for existing record of same user + type
            var existing = _ctx.CustomerDetails
                               .FirstOrDefault(cd => cd.UserId == userId
                                                  && cd.Type == dto.Type);

            if (existing != null)
            {
                // update
                existing.FullName = dto.FullName;
                existing.MobileNumber = dto.MobileNumber;
                existing.Address = dto.Address;
                existing.City = dto.City;
                existing.State = dto.State;
                existing.UpdatedDate = DateTime.UtcNow;

                _ctx.CustomerDetails.Update(existing);
                _ctx.SaveChanges();
                return (existing, false);
            }

            // insert new
            var created = new CustomerDetail
            {
                UserId = userId,
                FullName = dto.FullName,
                MobileNumber = dto.MobileNumber,
                Address = dto.Address,
                City = dto.City,
                State = dto.State,
                Type = dto.Type,
                CreatedDate = DateTime.UtcNow
            };
            _ctx.CustomerDetails.Add(created);
            _ctx.SaveChanges();
            return (created, true);
        }
    }
}
