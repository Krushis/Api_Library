

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LibraryBackend.Models
{
    public class UserSelectedBook
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }
        public int BookId { get; set; }

        [ForeignKey(nameof(BookId))]
        public Book Book { get; set; }

    }
}
