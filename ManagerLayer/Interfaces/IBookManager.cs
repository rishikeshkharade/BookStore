using System;
using System.Collections.Generic;
using System.Text;
using RepositoryLayer.Entity;
using RepositoryLayer.Models;

namespace ManagerLayer.Interfaces
{
    public interface IBookManager
    {
        Books AddBook(BookRequestModel book);
        Books UpdateBook(int bookId, BookRequestModel updatedBook);
        bool DeleteBook(int bookId);
        Books GetBookById(int bookId);
        IEnumerable<Books> GetAllBooks();
        IEnumerable<Books> SearchBooks(string keyword);
        IEnumerable<Books> SortBooks(string sortBy, string order);
    }
}
