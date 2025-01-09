

namespace LibraryBackend.Models
{
    public class Book
    {
        public string Year;
        public string Id;
        public string Title;
        public string Description;
        public double Price;

        public Book(string year, string Id, string title, string description)
        {
            this.Id = Id;
            this.Year = year;
            this.Title = title;
            this.Description = description;
        }

    }
}
