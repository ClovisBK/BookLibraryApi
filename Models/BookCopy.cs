using LibrarySystemApi;

namespace LibrarySystemApiV2.Models
{
    public class BookCopy
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public BookLibrary? Book { get; set; }
        public string BookNumber { get; set; } = string.Empty;
        public string Barcode { get; set; } = string.Empty;
        public BookCopyStatus Status { get; set; } 
    }
    public enum BookCopyStatus
    {
        Available,
        Loaned,
        Reserved,
        Lost,
        Damaged
    }
}
