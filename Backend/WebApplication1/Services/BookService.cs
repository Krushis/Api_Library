using LibraryBackend.Data;
using LibraryBackend.Models;

namespace LibraryBackend.Services
{
    public class BookService
    {
        private readonly LibraryContext _context;

        public BookService(LibraryContext context)
        {
            _context = context;
        }

        public IEnumerable<Book> GetAllBooks() => _context.Books.ToList();
        public Book GetBookById(int id) => _context.Books.Find(id);
        public void AddBook(Book book) => _context.Books.Add(book);
        public void SaveChanges() => _context.SaveChanges();
    }

}
