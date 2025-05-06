using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ManagerLayer.Interfaces
{
    public interface IMailService
    {
        Task<bool> SendResetTokenAsync(string toEmail, string token);
    }
}
