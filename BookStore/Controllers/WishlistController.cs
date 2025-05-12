using ManagerLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Models;
using RepositoryLayer.Entity;
using System.Linq;

namespace BookStore.Controllers
{
    [Route("api/wishlists")]
    [ApiController]
    public class WishlistController : ControllerBase
    {
        private readonly IWishlistManager _manager;
        public WishlistController(IWishlistManager manager)
        {
            _manager = manager;
        }

        // GET /api/wishlist
        [HttpGet]
        [Authorize(Roles = "User, Admin")]
        public IActionResult GetWishlist()
        {
            var items = _manager.GetAllWishlists();
            return Ok(new ResponseModel<IEnumerable<WishlistItemModel>>
            {
                IsSuccess = true,
                Message = items.Any()
                            ? "Wishlist retrieved successfully"
                            : "Your wishlist is empty",
                Data = items
            });
        }

        // POST /api/wishlist
        [HttpPost]
        [Authorize(Roles = "User, Admin")]
        public IActionResult AddToWishlist([FromBody] WishlistRequestModel model)
        {
            try
            {
                var item = _manager.AddToWishlist(model);
                return Ok(new ResponseModel<WishlistEntity>
                {
                    IsSuccess = true,
                    Message = "Book added to wishlist",
                    Data = item
                });
            }
            catch (Exception ex)
            {
                // duplicate case is a BadRequest
                if (ex.Message.Contains("already"))
                    return BadRequest(new ResponseModel<string>
                    {
                        IsSuccess = false,
                        Message = ex.Message
                    });

                return StatusCode(500, new ResponseModel<string>
                {
                    IsSuccess = false,
                    Message = ex.Message
                });
            }
        }

        // DELETE /api/wishlist/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "User, Admin")]
        public IActionResult RemoveFromWishlist(int id)
        {
            var ok = _manager.RemoveFromWishlist(id);
            if (!ok)
                return NotFound(new ResponseModel<string>
                {
                    IsSuccess = false,
                    Message = "Wishlist item not found"
                });

            return Ok(new ResponseModel<string>
            {
                IsSuccess = true,
                Message = "Wishlist item removed"
            });
        }
    }
}