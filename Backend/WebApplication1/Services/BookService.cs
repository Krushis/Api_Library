using LibraryBackend.Data;
using LibraryBackend.Models;

namespace LibraryBackend.Services
{
    /// <summary>
    /// Class for handling the business logic of the application
    /// </summary>
    public class BookService
    {
        private readonly LibraryContext _context;

        public BookService(LibraryContext context)
        {
            _context = context;
        }

        // Below methods use DbContext inherited methods
        public IEnumerable<Book> GetAllBooks() => _context.Books.ToList();
        public Book GetBookById(int id) => _context.Books.Find(id);
        public void AddBook(Book book) => _context.Books.Add(book);
        public void SaveChanges() => _context.SaveChanges();

        /// <summary>
        /// Used to show price when you click on the dynamically loaded page
        /// </summary>
        /// <param name="type"></param>
        /// <param name="days"></param>
        /// <param name="quick"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public double CalculateRentalPrice (string type, int days, bool quick)
        {
            double sum = 3; // Service fee

            switch (type)
            {
                case "Physical":
                    sum += days * 2;
                    break;
                case "Audiobook":
                    sum += days * 3;
                    break;
                default:
                    throw new Exception("type of book not correct");
            }
            
            if (days > 10)
            {
                sum = sum * 0.8;
            }
            else if (days > 3)
            {
                sum = sum * 0.9;
            }
            
            if (quick)
            {
                sum += 5;
            }

            return sum;
        }
    }



}
