using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;

namespace RepositoryLayer.Services
{
    public class BookRepository : IBookRepository
    {
        private readonly BookStoreDbContext _context;
        public BookRepository(BookStoreDbContext context)
        {
            _context = context;
        }
        public Books AddBook(Books book)
        {
            _context.Books.Add(book);
            _context.SaveChanges();
            return book;
        }
        public Books UpdateBook(int bookId, Books updatedbook)
        {
            var book = _context.Books.FirstOrDefault(b => b.BookId == bookId);
            if (book == null) return null;
            
            book.BookName = updatedbook.BookName;
            book.Author = updatedbook.Author;
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
    }
}
