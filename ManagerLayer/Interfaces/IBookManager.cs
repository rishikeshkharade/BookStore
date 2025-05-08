using System;
using System.Collections.Generic;
using System.Text;
using RepositoryLayer.Entity;

namespace ManagerLayer.Interfaces
{
    public interface IBookManager
    {
        Books AddBook(Books book);
        Books UpdateBook(int bookId, Books updatedBook);
        bool DeleteBook(int bookId);
        Books GetBookById(int bookId);
        IEnumerable<Books> GetAllBooks();
    }
}
