using System;
using System.Collections.Generic;
using System.Text;
using ManagerLayer.Interfaces;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Models;

namespace ManagerLayer.Services
{
    public class CustomerDetailManager : ICustomerDetailManager
    {
        private readonly ICustomerDetailRepository _customerDetailRepo;
        public CustomerDetailManager(ICustomerDetailRepository customerDetailRepo)
        {
            _customerDetailRepo = customerDetailRepo;
        }
            
        public (CustomerDetail Detail, bool WasCreated) Upsert(CustomerDetailRequestModel dto)
        {
            return _customerDetailRepo.Upsert(dto);
        }
    }
}
