using System;
using System.Collections.Generic;
using System.Text;
using RepositoryLayer.Entity;
using RepositoryLayer.Models;

namespace ManagerLayer.Interfaces
{
    public interface ICustomerDetailManager
    {
        (CustomerDetail Detail, bool WasCreated) Upsert(CustomerDetailRequestModel dto);
    }
}
