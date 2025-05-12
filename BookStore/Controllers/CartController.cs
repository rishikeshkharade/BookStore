using System;
using System.Collections.Generic;
using System.Linq;
using ManagerLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Entity;
using RepositoryLayer.Models;

namespace BookStore.Controllers
{
    [Route("api/cart")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartManager _cartManager;
        public CartController(ICartManager cartManager)
        {
            _cartManager = cartManager;
        }

        [HttpPost]
        [Authorize(Roles = "User, Admin")]
        public IActionResult AddToCart([FromBody] CartRequestModel cart)
        {
            try
            {
                var result = _cartManager.AddToCart(cart);
                return Ok(new ResponseModel<Cart> { IsSuccess = true, Message = "Book added to cart successfully", Data = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseModel<string> { IsSuccess = false, Message = ex.Message });
            }
        }

        [HttpGet]
        [Authorize(Roles = "User, Admin")]
        public IActionResult GetAllCartItems()
        {
            var items = _cartManager.GetCartItems() ?? Enumerable.Empty<Cart>();
            var total = items.Sum(c => c.TotalPrice);

            var response = new CartListResponseModel
            {
                Items = items,
                CartTotal = total
            };

            return Ok(new ResponseModel<CartListResponseModel>
            {
                IsSuccess = true,
                Message = items.Any() ? "Cart items retrieved successfully" : "Cart retrieved successfully but cart is empty",
                Data = response
            });
        }

        [HttpPut("{cartId}")]
        [Authorize(Roles = "User, Admin")]
       public IActionResult UpdateCartItem(int cartId, [FromBody] CartUpdateModel updateCart)
        {
            try
            {
                if (updateCart.Quantity <= 0)
                {
                    var deleted = _cartManager.DeleteCartItem(cartId);
                    if (!deleted)
                        return NotFound(new ResponseModel<string>
                        {
                            IsSuccess = false,
                            Message = "Cart item not found"
                        });
                    return Ok(new ResponseModel<string>
                    {
                        IsSuccess = true,
                        Message = "Cart item removed successfully"
                    });
                }
                var result = _cartManager.UpdateCart(cartId, updateCart.Quantity);
                if (result == null)
                {
                    return NotFound(new ResponseModel<string> { IsSuccess = false, Message = "Cart item not found" });
                }
                return Ok(new ResponseModel<Cart> { IsSuccess = true, Message = "Cart item updated successfully", Data = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseModel<string> { IsSuccess = false, Message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "User, Admin")]
        public IActionResult DeleteCartItem(int id)
        {
            try
            {
                var result = _cartManager.DeleteCartItem(id);
                if (!result)
                {
                    return NotFound(new ResponseModel<string> { IsSuccess = false, Message = "Cart item not found" });
                }
                return Ok(new ResponseModel<string> { IsSuccess = true, Message = "Cart item deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseModel<string> { IsSuccess = false, Message = ex.Message });
            }
        }

    }
}
