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

        public bool AddBookToSelection(string userId, Book book)
        {

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

            // prevents duplicate selections
            if (userSelection.SelectedBooks.Any(b => b.Id == book.Id))
            {
                return false; // Book already selected
            }

            userSelection.SelectedBooks.Add(book);
            _context.SaveChanges();
            return true;
        }

    }
}
