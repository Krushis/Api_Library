using LibraryBackend.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class UserSelection
{
    [Key]
    public int UserSelectionId { get; set; }

    public string UserId { get; set; }

    public List<UserSelectedBook> SelectedBooks { get; set; } = new List<UserSelectedBook>();

    public DateTime ReservationDate { get; set; } = DateTime.UtcNow;
    public DateTime ExpirationDate { get; set; }
}

