using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BookManagementAPI.Models;
using BookManagementAPI.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace BookManagementAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _repo;
        public BooksController(IBookRepository repo)
        {
            _repo = repo;
        }
        [HttpGet]
        public async Task<IActionResult> GetBooks(int pageNumber = 1, int pageSize = 10)
        {
            var books = await _repo.GetBooksTitlesPaginatedAsync(pageNumber, pageSize);
            var result = new List<object>();
            foreach (var book in books)
            {
                result.Add(new { book.Id, book.Title });
            }
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBook(int id)
        {
            var book = await _repo.GetBookByIdAsync(id);
            if (book == null)
                return NotFound();
            book.BookViews++;
            await _repo.UpdateBookAsync(book);
            var currentYear = DateTime.Now.Year;
            int yearsSincePublished = currentYear - book.PublicationYear;
            double popularityScore = book.BookViews * 0.5 + yearsSincePublished * 2;
            return Ok(new
            {
                book.Id,
                book.Title,
                book.AuthorName,
                book.PublicationYear,
                book.BookViews,
                YearsSincePublished = yearsSincePublished,
                PopularityScore = popularityScore
            });
        }
        [HttpPost]
        public async Task<IActionResult> CreateBook([FromBody] Book book)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (await _repo.BookExistsAsync(book.Title))
                return BadRequest("A book with the same title already exists.");
            var createdBook = await _repo.AddBookAsync(book);
            return CreatedAtAction(nameof(GetBook), new { id = createdBook.Id }, createdBook);
        }
        [HttpPost("bulk")]
        public async Task<IActionResult> CreateBooksBulk([FromBody] IEnumerable<Book> books)
        {
            foreach (var book in books)
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                if (await _repo.BookExistsAsync(book.Title))
                    return BadRequest($"A book with the title '{book.Title}' already exists.");
            }
            var createdBooks = await _repo.AddBooksBulkAsync(books);
            return Ok(createdBooks);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] Book updatedBook)
        {
            if (id != updatedBook.Id)
                return BadRequest("ID mismatch.");
            var existingBook = await _repo.GetBookByIdAsync(id);
            if (existingBook == null)
                return NotFound();
            if (!existingBook.Title.Equals(updatedBook.Title, StringComparison.OrdinalIgnoreCase) &&
                await _repo.BookExistsAsync(updatedBook.Title))
                return BadRequest("A book with the same title already exists.");
            existingBook.Title = updatedBook.Title;
            existingBook.AuthorName = updatedBook.AuthorName;
            existingBook.PublicationYear = updatedBook.PublicationYear;
            var result = await _repo.UpdateBookAsync(existingBook);
            if (!result)
                return StatusCode(500, "An error occurred while updating the book.");
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> SoftDeleteBook(int id)
        {
            var result = await _repo.SoftDeleteBookAsync(id);
            if (!result)
                return NotFound();
            return NoContent();
        }
        [HttpDelete("bulk")]
        public async Task<IActionResult> SoftDeleteBooksBulk([FromBody] IEnumerable<int> ids)
        {
            var result = await _repo.SoftDeleteBooksBulkAsync(ids);
            if (!result)
                return NotFound("None of the provided book IDs were found.");
            return NoContent();
        }
    }
}
