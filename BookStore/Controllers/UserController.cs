using System;
using System.Threading.Tasks;
using CommonLayer.Models;
using ManagerLayer.Interfaces;
using ManagerLayer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Entity;

namespace BookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserManager userManager;
        private readonly IMailService _mailService;
        public UserController(IUserManager userManager, IMailService mailService)
        {
            this.userManager = userManager;
            _mailService = mailService;
        }

        [HttpPost("Register")]
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

        [HttpPost("Login")]
        [AllowAnonymous]
        public IActionResult Login([FromBody] UserLoginModel model)
        {
            try
            {
                var result = userManager.Login(model);
                if (result != null)
                {
                    return Ok(new ResponseModel<UserEntity> { IsSuccess = true, Message = "User logged in successfully", Data = result });
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

        [HttpPost("ForgetPassword")]
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
    }
}
