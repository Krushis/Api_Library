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
            var alreadySelected = _context.UserSelectedBooks
                .Any(b => b.UserId == userId && b.BookId == bookId);

            if (alreadySelected)
            {
                _logger.LogInformation($"User {userId} already selected book {bookId}");
                return false;
            }

            var book = _context.Books.FirstOrDefault(b => b.Id == bookId);
            if (book == null)
            {
                _logger.LogWarning($"Book with ID {bookId} not found");
                return false;
            }

            var selectedBook = new UserSelectedBook(book, userId, bookId, selectedId, price);

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

        /// <summary>
        /// Deselects a book from a user's - specified by the UserId parameter, selected books list
        /// </summary>
        /// <param name="bookId">Book whose Id we want to remove from selections</param>
        /// <param name="userId">The Id of the User whose selections we want to go through</param>
        public void DeselectBook(int bookId, string userId)
        {
            var selectedBook = _context.UserSelectedBooks
            .FirstOrDefault(usb => usb.BookId == bookId && usb.UserId == userId);

            if (selectedBook != null)
            {
                _context.UserSelectedBooks.Remove(selectedBook);
                _context.SaveChanges();
            }
        }

    }
}
