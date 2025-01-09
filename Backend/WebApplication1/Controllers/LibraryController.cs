using LibraryBackend.Models;
using LibraryBackend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibraryController : ControllerBase
    {
        private readonly BookService _bookService;

        public LibraryController(BookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
        public IActionResult GetAllBooks()
        {
            var books = _bookService.GetAllBooks();
            if (books == null || !books.Any())
            {
                return NotFound("No books found");
            }
            return Ok(books);
        }

        [HttpGet("{id}")]
        public IActionResult GetBook(int id)
        {
            var book = _bookService.GetBookById(id);
            if (book != null)
            {
                return Ok(book);
            }
            return NotFound("Book not found");
        }

        [HttpPost]
        public IActionResult AddBook([FromBody] Book book)
        {
            _bookService.AddBook(book);
            _bookService.SaveChanges();
            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
        }
    }
}
