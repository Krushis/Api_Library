﻿using LibraryBackend.Models;
using System.Linq;
using LibraryBackend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibraryController : ControllerBase
    {
        private readonly BookService _bookService;
        private readonly ILogger<LibraryController> _logger;
        private readonly UserService _userService;

        public LibraryController(BookService bookService, ILogger<LibraryController> logger, UserService userServ)
        {
            _userService = userServ;
            _bookService = bookService;
            _logger = logger;

        }

        [HttpGet]
        public IActionResult GetAllBooks()
        {
            var books = _bookService.GetAllBooks();
            if (books == null || !books.Any())
            {
                return Ok(new List<Book>());
            }
            _logger.LogInformation("Fetched all books from database");
            return Ok(books);
        }

        // Selects a book with price based on the Form results
        [HttpGet("{id}")]
        public IActionResult GetBook(int id, [FromQuery] int days=1, [FromQuery] string type="Physical", [FromQuery] bool quick=false)
        {
            //_logger.LogInformation($"Next log is book");
            //_logger.LogInformation($"{id}, {days}, {type}, {quick}.");
            var book = _bookService.GetBookById(id);
            //_logger.LogInformation($"{book.Id}");
            if (book != null)
            {
                double price = _bookService.CalculateRentalPrice(type, days, quick);

                var response = new
                {
                    Book = book,
                    Price = price
                };
                
                _logger.LogInformation($"Fetched book with id of {id}, with price {price}");

                SelectBook(id, price); // problem is with this line

                return Ok(response);
            }

            return NotFound("Book not found");
        }

        [HttpPost("add-book")]
        public async Task<IActionResult> AddBookWithImage([FromForm] Book book, [FromForm] IFormFile coverImage)
        {
            _logger.LogInformation("Reached method");
            // can implement IFormCollection if I ever want to add multiple images to the book
            if (coverImage == null || coverImage.Length == 0)
            {
                return BadRequest("No image file uploaded.");
            }

            var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
            // Guid is a massive unique identifier
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(coverImage.FileName);
            var filePath = Path.Combine(uploadFolder, fileName);

            if (!Directory.Exists(uploadFolder))
            {
                Directory.CreateDirectory(uploadFolder);
            }

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await coverImage.CopyToAsync(stream);
            }

            // Update the book with the cover image path
            book.ImagePath = "/images/" + fileName;

            _bookService.AddBook(book);
            _bookService.SaveChanges();
            _logger.LogInformation($"Added book with id: {book.Id}");
            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
        }

        [HttpPost("{id}/select")]
        public IActionResult SelectBook(int id, double price)
        {
            var book = _bookService.GetBookById(id);

            if (book == null)
            {
                return NotFound("Book not found");
            }

            string userId = "AJAJ"; // implement validation for users
            int selectedId = Guid.NewGuid().GetHashCode();


            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User must be logged in to select books");
            }

            var success = _userService.AddBookToSelection(_logger, userId, id, selectedId, price);
            if (!success)
            {
                return BadRequest("Book already selected");
            }
            _logger.LogInformation($"Selected book with id: {id}");
            return Ok($"Book with ID {id} successfully added to selection.");
        }

        [HttpGet("{userId}/selected-books")]
        public IActionResult GetSelectedBooks(string userId)
        {

            var books = _userService.GetSelectedBooksByUser(userId);
            if (books == null || books.Count == 0)
            {
                return NotFound();
            }
            _logger.LogInformation($"Fetched {books.Count()} books from DB");
            return Ok(books);
        }

        [HttpDelete("{userId}/deselect/{bookId}")]
        public IActionResult DeselectBook(string userId, int BookId)
        {
            _logger.LogInformation($"Trying to deselect book with id: {BookId}");
            _userService.DeselectBook(BookId, userId);
        
            return Ok();
        }

    }
}
