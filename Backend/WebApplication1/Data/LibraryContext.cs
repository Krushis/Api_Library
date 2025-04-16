using Microsoft.EntityFrameworkCore;
using LibraryBackend.Models;

namespace LibraryBackend.Data
{
    public class LibraryContext : DbContext
    {
        public DbSet<Book> Books {  get; set; }
        public DbSet<UserSelection> UserSelections { get; set; }
        public DbSet<UserSelectedBook> UserSelectedBooks { get; set; }

        public LibraryContext(DbContextOptions<LibraryContext> options) : base(options) 
        {
            
        }



    }
}
