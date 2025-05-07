using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Models
{
    public class ForgetPasswordModel
    {
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
