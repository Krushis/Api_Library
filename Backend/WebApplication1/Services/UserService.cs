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

        public bool AddBookToSelection(string userId, UserSelectedBook book)
        {
            var userSelection = _context.UserSelections
                .Include(us => us.SelectedBooks)
                .FirstOrDefault(us => us.UserId == userId);

            if (userSelection == null)
            {
                userSelection = new UserSelection
                {
                    UserId = userId,
                    SelectedBooks = new List<UserSelectedBook>()
                };
                _context.UserSelections.Add(userSelection);
            }

            // prevent duplicates
            if (userSelection.SelectedBooks.Any(b => b.Id == book.Id))
            {
                return false;
            }

            userSelection.SelectedBooks.Add(book);
            _context.SaveChanges();
            return true;
        }


        public List<UserSelectedBook> GetSelectedBooksByUser(string userId)
        {
            var selection = _context.UserSelections
                .Include(us => us.SelectedBooks)
                .FirstOrDefault(us => us.UserId == userId);

            return selection?.SelectedBooks ?? new List<UserSelectedBook>();
        }

    }
}
