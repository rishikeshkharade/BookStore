using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Models
{
    public class ResetPasswordModel
    {
        public string Email { get; set; }
        public string NewPassword { get; set; }
        public string Token { get; set; }
    }
}
