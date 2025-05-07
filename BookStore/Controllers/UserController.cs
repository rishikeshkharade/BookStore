using System;
using System.Threading.Tasks;
using ManagerLayer.Interfaces;
using ManagerLayer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Entity;
using RepositoryLayer.Models;

namespace BookStore.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserManager userManager;
        private readonly MailService _mailService;
        public UserController(IUserManager userManager, MailService mailService)
        {
            this.userManager = userManager;
            _mailService = mailService;
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Register([FromBody] UserRegisterModel model)
        {
            try
            {
                var result = userManager.Register(model);
                if (result != null)
                {
                    return Ok(new ResponseModel<UserEntity> {IsSuccess = true, Message = "User registered successfully", Data = result });
                }
                else
                {
                    return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "User already exists" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public IActionResult Login([FromBody] UserLoginModel model)
        {
            try
            {
                var result = userManager.Login(model);
                if (result != null)
                {
                    return Ok(new ResponseModel<string> { IsSuccess = true, Message = "User logged in successfully", Data = result });
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

        [HttpPost("forgot")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgetPassword(string email)
        {
            try
            {
                var userExists = userManager.EmailChecker(email);
                if (!userExists)
                {
                    return BadRequest(new ResponseModel<string>
                    {
                        IsSuccess = false,
                        Message = "Email not registered"
                    });
                }

                var forgetPasswordModel = userManager.ForgetPassword(email);


                // Send reset token via email
                
                    var emailSent = await _mailService.SendResetTokenAsync(forgetPasswordModel.Email, forgetPasswordModel.Token);
                //return Ok("Token generated successfully");

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

        [HttpPost("reset")]
        [AllowAnonymous]
        public IActionResult ResetPassword([FromBody] ResetPasswordModel model)
        {
            try
            {
                var result = userManager.ResetPassword(model);
                if (result)
                {
                    return Ok(new ResponseModel<string> { IsSuccess = true, Message = "Password reset successfully" });
                }
                else
                {
                    return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Invalid token or email" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
