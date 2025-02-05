namespace LibraryBackend.Models
{
    /// <summary>
    /// For handling the transfer of data from frontend to backend of book reservation
    /// </summary>
    public class PriceDTO
    {
        public string BookType { get; set; }
        public int Days { get; set; }
        public bool QuickPickUp { get; set; }
    }
}
