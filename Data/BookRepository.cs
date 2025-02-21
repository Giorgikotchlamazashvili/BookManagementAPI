using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BookManagementAPI.Data;
using BookManagementAPI.Models;

namespace BookManagementAPI.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly BookDbContext _context;
        public BookRepository(BookDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Book>> GetAllBooksAsync()
        {
            return await _context.Books.Where(b => !b.IsDeleted).ToListAsync();
        }
        public async Task<IEnumerable<Book>> GetBooksTitlesPaginatedAsync(int pageNumber, int pageSize)
        {
            var currentYear = System.DateTime.Now.Year;
            var books = await _context.Books.Where(b => !b.IsDeleted).ToListAsync();
            var sortedBooks = books.Select(b => new {
                    Book = b,
                    PopularityScore = b.BookViews * 0.5 + ((currentYear - b.PublicationYear) * 2)
                })
                .OrderByDescending(x => x.PopularityScore)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(x => x.Book)
                .ToList();
            return sortedBooks;
        }
        public async Task<Book> GetBookByIdAsync(int id)
        {
            return await _context.Books.FirstOrDefaultAsync(b => b.Id == id && !b.IsDeleted);
        }
        public async Task<Book> AddBookAsync(Book book)
        {
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();
            return book;
        }
        public async Task<IEnumerable<Book>> AddBooksBulkAsync(IEnumerable<Book> books)
        {
            await _context.Books.AddRangeAsync(books);
            await _context.SaveChangesAsync();
            return books;
        }
        public async Task<bool> UpdateBookAsync(Book book)
        {
            _context.Books.Update(book);
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<bool> SoftDeleteBookAsync(int id)
        {
            var book = await GetBookByIdAsync(id);
            if (book == null)
                return false;
            book.IsDeleted = true;
            _context.Books.Update(book);
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<bool> SoftDeleteBooksBulkAsync(IEnumerable<int> ids)
        {
            var books = await _context.Books.Where(b => ids.Contains(b.Id) && !b.IsDeleted).ToListAsync();
            if (!books.Any())
                return false;
            foreach (var book in books)
            {
                book.IsDeleted = true;
            }
            _context.Books.UpdateRange(books);
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<bool> BookExistsAsync(string title)
        {
            return await _context.Books.AnyAsync(b => b.Title == title && !b.IsDeleted);
        }
    }
}
