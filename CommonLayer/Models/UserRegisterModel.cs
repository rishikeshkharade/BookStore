using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLayer.Models
{
    public class UserRegisterModel
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
    }
}
