using Microsoft.EntityFrameworkCore;
using LibraryBackend.Models;

namespace LibraryBackend.Data
{
    public class LibraryContext : DbContext
    {
        public DbSet<Book> Books {  get; set; }

        public LibraryContext(DbContextOptions<LibraryContext> options) : base(options) 
        {
            
        }



    }
}
