using System;
using System.Collections.Generic;
using System.Text;
using ManagerLayer.Interfaces;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Models;

namespace ManagerLayer.Services
{
    public class BookManager : IBookManager
    {
        private readonly IBookRepository _bookRepository;
        public BookManager(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }
        public Books AddBook(BookRequestModel book)
        {
            return _bookRepository.AddBook(book);
        }
        public bool DeleteBook(int bookId)
        {
            return _bookRepository.DeleteBook(bookId);
        }
        public IEnumerable<Books> GetAllBooks()
        {
            return _bookRepository.GetAllBooks();
        }
        public Books GetBookById(int bookId)
        {
            return _bookRepository.GetBookById(bookId);
        }
        public Books UpdateBook(int bookId, BookRequestModel updatedBook)
        {
            return _bookRepository.UpdateBook(bookId, updatedBook);
        }
        public IEnumerable<Books> SearchBooks(string keyword)
        {
            return _bookRepository.SearchBooks(keyword);
        }
        public IEnumerable<Books> SortBooks(string sortBy, string order)
        {
            return _bookRepository.SortBooks(sortBy, order);
        }
        public IEnumerable<Books> GetRecentlyAddedBooks()
        {
            return _bookRepository.GetRecentlyAddedBooks();
        }
    }
}
