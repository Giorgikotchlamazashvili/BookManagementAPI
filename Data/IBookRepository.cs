using System.Collections.Generic;
using System.Threading.Tasks;
using BookManagementAPI.Models;

namespace BookManagementAPI.Repositories
{
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> GetAllBooksAsync();
        Task<IEnumerable<Book>> GetBooksTitlesPaginatedAsync(int pageNumber, int pageSize);
        Task<Book> GetBookByIdAsync(int id);
        Task<Book> AddBookAsync(Book book);
        Task<IEnumerable<Book>> AddBooksBulkAsync(IEnumerable<Book> books);
        Task<bool> UpdateBookAsync(Book book);
        Task<bool> SoftDeleteBookAsync(int id);
        Task<bool> SoftDeleteBooksBulkAsync(IEnumerable<int> ids);
        Task<bool> BookExistsAsync(string title);
    }
}
