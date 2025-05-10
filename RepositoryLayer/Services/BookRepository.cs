using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Models;

namespace RepositoryLayer.Services
{
    public class BookRepository : IBookRepository
    {
        private readonly BookStoreDbContext _context;


        public BookRepository(BookStoreDbContext context)
        {
            _context = context;
        }
        public Books AddBook(BookRequestModel book)
        {
            var newBook = new Books
            {
                BookName = book.BookName,
                Author = book.AuthorName,
                Price = book.Price,
                Quantity = book.Quantity,
                Description = book.Description,
                BookImage = book.BookImage,
                DiscountPrice = book.DiscountPrice,
                CreatedAt = DateTime.Now
            };

            _context.Books.Add(newBook);
            _context.SaveChanges();
            return newBook;
        }
        public Books UpdateBook(int bookId, BookRequestModel updatedbook)
        {
            var book = _context.Books.FirstOrDefault(b => b.BookId == bookId);
            if (book == null) return null;
            
            book.BookName = updatedbook.BookName;
            book.Author = updatedbook.AuthorName;
            book.Price = updatedbook.Price;
            book.Quantity = updatedbook.Quantity;
            book.Description = updatedbook.Description;
            book.BookImage = updatedbook.BookImage;
            book.DiscountPrice = updatedbook.DiscountPrice;
            book.UpdatedAt = DateTime.Now;
  
            _context.SaveChanges();
            return book;
        }
        public bool DeleteBook(int bookId)
        {
            var book = _context.Books.Find(bookId);
            if (book == null) return false;
            _context.Books.Remove(book);
            _context.SaveChanges();
            return true;
        }
        public Books GetBookById(int bookId)
        {
            return _context.Books.Find(bookId);
        }
        public IEnumerable<Books> GetAllBooks()
        {
            return _context.Books.ToList();
        }

        public IEnumerable<Books> SearchBooks(string keyword)
        {
            keyword = keyword?.ToLower() ?? "";

            return _context.Books.Where(b => b.BookName.Contains(keyword) || b.Author.Contains(keyword)).ToList();
        }

        public IEnumerable<Books> SortBooks(string sortBy, string order)
        {
            var books = _context.Books.AsQueryable();

            sortBy = sortBy?.ToLower() ?? "price";
            order = order?.ToLower() ?? "asc";

            switch (sortBy)
            {
                case "price":
                    books = order == "asc" ? books.OrderBy(b => b.Price) : books.OrderByDescending(b => b.Price);
                    break;
                case "name":
                    books = order == "asc" ? books.OrderBy(b => b.BookName) : books.OrderByDescending(b => b.BookName);
                    break;
                case "author":
                    books = order == "asc" ? books.OrderBy(b => b.Author) : books.OrderByDescending(b => b.Author);
                    break;
                default:
                    books = order == "asc" ? books.OrderBy(b => b.Price) : books.OrderByDescending(b => b.Price);
                    break;
            }
            return books.ToList();
        }
    }
}
