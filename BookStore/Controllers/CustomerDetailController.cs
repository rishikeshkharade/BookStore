using System;
using ManagerLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Models;

namespace BookStore.Controllers
{
    [ApiController]
    [Route("api/customers")]
    public class CustomerDetailController : ControllerBase
    {
        private readonly ICustomerDetailManager _manager;

        public CustomerDetailController(ICustomerDetailManager manager)
        {
            _manager = manager;
        }

        [HttpPost]
        [Authorize(Roles = "User, Admin")]
        public IActionResult Upsert([FromBody] CustomerDetailRequestModel dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var (detail, wasCreated) = _manager.Upsert(dto);

                var response = new CustomerDetailResponseModel
                {
                    CustomerDetailId = detail.CustomerDetailId,
                    FullName = detail.FullName,
                    MobileNumber = detail.MobileNumber,
                    Address = detail.Address,
                    City = detail.City,
                    State = detail.State,
                    Type = detail.Type,
                    CreatedDate = detail.CreatedDate,
                    UpdatedDate = detail.UpdatedDate
                };

                return Ok(new ResponseModel<CustomerDetailResponseModel>
                {
                    IsSuccess = true,
                    Message = wasCreated
                                ? "Details saved successfully"
                                : "Details updated successfully",
                    Data = response
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseModel<string>
                {
                    IsSuccess = false,
                    Message = ex.Message
                });
            }
        }
    }
}