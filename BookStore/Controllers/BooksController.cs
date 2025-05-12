using System;
using System.Collections.Generic;
using ManagerLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Entity;
using RepositoryLayer.Models;

namespace BookStore.Controllers
{
    [Route("api/books")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookManager _bookManager;
        public BooksController(IBookManager bookManager)
        {
            _bookManager = bookManager;
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult AddBook([FromBody] BookRequestModel book)
        {
            try
            {
                var result = _bookManager.AddBook(book);
                return Ok(new ResponseModel<Books> { IsSuccess = true, Message = "Book added successfully", Data = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseModel<string> { IsSuccess = false, Message = ex.Message });
            }
        }

        [HttpGet]
        [Authorize(Roles = "User, Admin")]
        public IActionResult GetAllBooks()
        {
            var books = _bookManager.GetAllBooks();
            return Ok(new ResponseModel<IEnumerable<Books>> { IsSuccess = true, Message = "Books retrieved successfully", Data = books });
        }


        [HttpGet("{id}")]
        [Authorize(Roles = "User, Admin")]
        public IActionResult GetBookById(int id)
        {
            try
            {
                var book = _bookManager.GetBookById(id);
                if (book == null)
                {
                    return NotFound(new ResponseModel<string> { IsSuccess = false, Message = "Book not found" });
                }
                return Ok(new ResponseModel<Books> { IsSuccess = true, Message = "Book retrieved successfully", Data = book });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseModel<string> { IsSuccess = false, Message = ex.Message });
            }
        }


       


        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateBook(int id, [FromBody] BookRequestModel updatedBook)
        {
            try
            {
                var book = _bookManager.UpdateBook(id, updatedBook);
                if (book == null)
                {
                    return NotFound(new ResponseModel<string> { IsSuccess = false, Message = "Book not found" });
                }
                return Ok(new ResponseModel<Books> { IsSuccess = true, Message = "Book Updated Successfully", Data = book });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseModel<string> { IsSuccess = false, Message = ex.Message });
            }
        }



        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]

        public IActionResult DeleteBook(int id)
        {
            var result = _bookManager.DeleteBook(id);
            if (!result) {
                return NotFound(new ResponseModel<string> { IsSuccess = false, Message = "Book not found or already deleted" });
        }
            return Ok(new ResponseModel<string> { IsSuccess = true, Message = "Book deleted successfully" });
        }

        [HttpGet("search")]
        [Authorize(Roles = "User, Admin")]
        public IActionResult SearchBooks([FromQuery] string keyword)
        {
            try
            {
                var books = _bookManager.SearchBooks(keyword);
                return Ok(new ResponseModel<IEnumerable<Books>> { IsSuccess = true, Message = "Books retrieved successfully", Data = books });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseModel<string> { IsSuccess = false, Message = $"Internal server error: {ex.Message}" });
            }
        }

        [HttpGet("sort")]
        [Authorize(Roles = "User, Admin")]
        public IActionResult SortBooks([FromQuery] string sortBy, [FromQuery] string order)
        {
            try
            {
                var books = _bookManager.SortBooks(sortBy, order);
                return Ok(new ResponseModel<IEnumerable<Books>> { IsSuccess = true, Message = "Books sorted successfully", Data = books });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseModel<string> { IsSuccess = false, Message = $"Internal server error: {ex.Message}" });
            }
        }

        [HttpGet("recent")]
        [Authorize(Roles = "User, Admin")]
        public IActionResult GetRecentlyAdded()
        {
            try
            {
                var books = _bookManager.GetRecentlyAddedBooks();
                return Ok(new ResponseModel<IEnumerable<Books>> { IsSuccess = true, Message = "Recently added books retrieved successfully", Data = books });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseModel<string> { IsSuccess = false, Message = $"Internal server error: {ex.Message}" });
            }
        }
    }
}
