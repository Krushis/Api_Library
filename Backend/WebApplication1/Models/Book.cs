using System.ComponentModel.DataAnnotations;

namespace LibraryBackend.Models
{
    /// <summary>
    /// Simple class to save information about a book
    /// </summary>
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Year { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }

        public string? ImagePath { get; set; }

        public Book() { }

        public Book(int id, string year, string title, string description)
        {
            Id = id;
            Year = year;
            Title = title;
            Description = description;
        }
    }

}
