using LibraryBackend.Controllers;
using LibraryBackend.Data;
using LibraryBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryBackend.Services
{
    /// <summary>
    /// Class for User book saving logic 
    /// </summary>
    public class UserService
    {

        private readonly LibraryContext _context;

        public UserService(LibraryContext context)
        {
            _context = context;
        }

        public bool AddBookToSelection(ILogger<LibraryController> _logger, string userId, int bookId, int selectedId, double price)
        {
            // Check if the book already exists in the user's selection
            var alreadySelected = _context.UserSelectedBooks
                .Any(b => b.UserId == userId && b.BookId == bookId);

            if (alreadySelected)
            {
                _logger.LogInformation($"User {userId} already selected book {bookId}");
                return false;
            }

            // Get the book from the DB
            var book = _context.Books.FirstOrDefault(b => b.Id == bookId);
            if (book == null)
            {
                _logger.LogWarning($"Book with ID {bookId} not found");
                return false;
            }

            // Create a new UserSelectedBook
            var selectedBook = new UserSelectedBook(book, userId, bookId, selectedId, price);

            // Add it to the database
            _context.UserSelectedBooks.Add(selectedBook);
            _context.SaveChanges();

            _logger.LogInformation($"Book {bookId} added to selection for user {userId}");
            return true;
        }


        public List<UserSelectedBook> GetSelectedBooksByUser(string userId)
        {
            return _context.UserSelectedBooks
                .Where(usb => usb.UserId == userId)
                .Include(usb => usb.Book)
                .ToList();
        }


    }
}
