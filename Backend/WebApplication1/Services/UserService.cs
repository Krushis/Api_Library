
using LibraryBackend.Data;
using LibraryBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryBackend.Services
{
    public class UserService
    {

        private readonly LibraryContext _context;

        public UserService(LibraryContext context)
        {
            _context = context;
        }

        public bool AddBookToSelection(string userId, Book book)
        {
            // Fetch or create a new UserSelection entry
            var userSelection = _context.UserSelections.FirstOrDefault(us => us.UserId == userId);
            if (userSelection == null)
            {
                userSelection = new UserSelection
                {
                    UserId = userId,
                    SelectedBooks = new List<Book>()
                };
                _context.UserSelections.Add(userSelection);
            }

            // Prevent duplicate selections
            if (userSelection.SelectedBooks.Any(b => b.Id == book.Id))
            {
                return false; // Book already selected
            }

            // Add the book to the selection
            userSelection.SelectedBooks.Add(book);
            _context.SaveChanges();
            return true;
        }

    }
}
