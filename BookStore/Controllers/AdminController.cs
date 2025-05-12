using System;
using System.Threading.Tasks;
using RepositoryLayer.Models;
using ManagerLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Entity;
using ManagerLayer.Services;

namespace BookStore.Controllers
{
    [Route("api/admins")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminManager _adminManager;
        private readonly MailService _mailService;
        public AdminController(IAdminManager adminManager, MailService mailService)
        {
            _adminManager = adminManager;
            _mailService = mailService;
        }

        [HttpPost]
        public IActionResult Register([FromBody] AdminRegisterModel model)
        {
            try
            {
                var result = _adminManager.Register(model);
                if (result != null)
                {
                    return Ok(new ResponseModel<AdminEntity> { IsSuccess = true, Message = "Admin registered successfully", Data = result });
                }
                else
                {
                    return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Admin already exists" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] AdminLoginModel model)
        {
            try
            {
                var result = _adminManager.Login(model);
                if (result != null)
                {
                    return Ok(new ResponseModel<AdminLoginResponseModel> { IsSuccess = true, Message = "Admin logged in successfully", Data = result });
                }
                else
                {
                    return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Invalid credentials" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost("forgot-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            try
            {
                var token = _adminManager.ForgetPassword(email);

                if (token == null)
                {
                    return BadRequest(new ResponseModel<string>
                    {
                        IsSuccess = false,
                        Message = "Email not registered"
                    });
                }

                var emailSent = await _mailService.SendResetTokenAsync(email, token);

                if (!emailSent)
                {
                    return StatusCode(500, new ResponseModel<string>
                    {
                        IsSuccess = false,
                        Message = "Failed to send email"
                    });
                }

                return Ok(new ResponseModel<string>
                {
                    IsSuccess = true,
                    Message = "Reset token sent to your email"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseModel<string>
                {
                    IsSuccess = false,
                    Message = $"Internal server error: {ex.Message}"
                });
            }
        }

        
        [HttpPost("reset-password")]
        [AllowAnonymous]
        public IActionResult ResetPassword([FromBody] ResetPasswordModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            bool success = _adminManager.ResetPassword(model);

            if (!success)
            {
                return BadRequest(new ResponseModel<string>
                {
                    IsSuccess = false,
                    Message = "Invalid or expired token or same as old password"
                });
            }

            return Ok(new ResponseModel<string>
            {
                IsSuccess = true,
                Message = "Password reset successful"
            });
        }

        [HttpPost("refresh-token")]
        [AllowAnonymous]
        public IActionResult RefreshToken([FromBody] RefreshRequestModel model)
        {
            if (string.IsNullOrEmpty(model.RefreshToken))
            {
                return BadRequest(new ResponseModel<string>
                {
                    IsSuccess = false,
                    Message = "Refresh token is required"
                });
            }

            var newTokens = _adminManager.Refresh(model.RefreshToken);

            if (newTokens == null)
            {
                return Unauthorized(new ResponseModel<string>
                {
                    IsSuccess = false,
                    Message = "Invalid or expired refresh token"
                });
            }

            return Ok(new ResponseModel<TokenModel>
            {
                IsSuccess = true,
                Message = "Token refreshed successfully",
                Data = newTokens
            });
        }

    }
}
