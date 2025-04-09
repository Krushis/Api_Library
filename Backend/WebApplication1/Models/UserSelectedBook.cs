using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LibraryBackend.Models
{
    public class UserSelectedBook
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }
        public int BookId { get; set; }

        [ForeignKey(nameof(BookId))]
        public Book Book { get; set; }

        public UserSelectedBook(Book book, string userid, int bookid, int id) 
        {
            this.Book = book;
            this.UserId = userid;
            this.BookId = bookid;
            this.Id = id;
        }

        public UserSelectedBook() { }

    }
}
