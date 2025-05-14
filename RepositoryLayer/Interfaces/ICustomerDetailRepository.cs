using System;
using System.Collections.Generic;
using System.Text;
using RepositoryLayer.Entity;
using RepositoryLayer.Models;

namespace RepositoryLayer.Interfaces
{
     public interface ICustomerDetailRepository
    {
        (CustomerDetail Detail, bool WasCreated) Upsert(CustomerDetailRequestModel dto);

    }
}
