using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RepositoryLayer.Models
{
    public class ResetPasswordModel
    {
       
        public string Email { get; set; }
        [Required(ErrorMessage = "New password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "Token is required")]
        public string Token { get; set; }
    }
}
