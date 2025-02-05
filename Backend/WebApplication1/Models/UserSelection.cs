using LibraryBackend.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class UserSelection
{
    [Key]
    public int UserSelectionId { get; set; }  // unique id for the selection of a book
    public string UserId { get; set; } // the id for the user who took the book
    public List<Book> SelectedBooks { get; set; } = new List<Book>();

    public DateTime ReservationDate { get; set; } = DateTime.UtcNow;
    public DateTime ExpirationDate {  get; set; }

}
