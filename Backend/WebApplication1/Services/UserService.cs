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

        public bool AddBookToSelection(string userId, int bookId, int selectedId, double price)
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

            if (userSelection.SelectedBooks.Any(b => b.BookId == bookId))
            {
                return false;
            }

            var book = _context.Books.FirstOrDefault(b => b.Id == bookId);

            if (book == null)
                return false;

            var selectedBook = new UserSelectedBook(book, userId, bookId, selectedId, price);

            userSelection.SelectedBooks.Add(selectedBook);
            _context.SaveChanges();
            return true;
        }




        public List<UserSelectedBook> GetSelectedBooksByUser(string userId)
        {
            var selection = _context.UserSelections
                .Where(us => us.UserId == userId)
                .SelectMany(us => us.SelectedBooks)
                .Include(usb => usb.Book)            
                .ToList();                    
            return selection;
        }



    }
}
