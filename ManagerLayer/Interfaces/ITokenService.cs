using System;
using System.Collections.Generic;
using System.Text;

namespace ManagerLayer.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(int id, string email, string role);
    }
}
