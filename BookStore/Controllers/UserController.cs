using System;
using System.Threading.Tasks;
using ManagerLayer.Interfaces;
using ManagerLayer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Models;
using RepositoryLayer.Services;

namespace BookStore.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserManager userManager;
        private readonly MailService _mailService;
        private readonly TokenService _tokenService;

        public UserController(IUserManager userManager, MailService mailService, TokenService tokenService)
        {
            this.userManager = userManager;
            _mailService = mailService;
            _tokenService = tokenService;
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
                    return Ok(new ResponseModel<UserResponseModel> { IsSuccess = true, Message = "User registered successfully", Data = result });
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
                    return Ok(new ResponseModel<TokenModel> { IsSuccess = true, Message = "User logged in successfully", Data = result });
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
                var token = userManager.ForgetPassword(email);  // already uses TokenService

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

        [HttpPost("refresh-token")]
        public IActionResult RefreshToken([FromBody] RefreshRequestModel model)
        {
            var tokenModel = userManager.Refresh(model.RefreshToken);

            if (tokenModel == null)
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
                Data = tokenModel
            });
        }


    }
}
