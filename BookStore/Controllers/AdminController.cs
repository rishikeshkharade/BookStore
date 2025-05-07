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
                    return Ok(new ResponseModel<string> { IsSuccess = true, Message = "Admin logged in successfully", Data = result });
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
            var data = _adminManager.ForgetPassword(email);
            if (data == null)
                return NotFound(new ResponseModel<string> { IsSuccess = false, Message = "Admin email not found" });

            var success = await _mailService.SendResetTokenAsync(data.Email, data.Token);
            if (!success)
                return StatusCode(500, new ResponseModel<string> { IsSuccess = false, Message = "Failed to send email" });

            return Ok(new ResponseModel<string> { IsSuccess = true, Message = "Reset token sent to admin email" });
        }
    }
}
