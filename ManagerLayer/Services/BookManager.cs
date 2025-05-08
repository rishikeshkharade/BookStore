using System;
using System.Collections.Generic;
using System.Text;
using ManagerLayer.Interfaces;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;

namespace ManagerLayer.Services
{
    public class BookManager : IBookManager
    {
        private readonly IBookRepository _bookRepository;
        public BookManager(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }
        public Books AddBook(Books book)
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
        public Books UpdateBook(int bookId, Books updatedBook)
        {
            return _bookRepository.UpdateBook(bookId, updatedBook);
        }
    }
}
