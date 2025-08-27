using LibrarySystemApiV2.Models;

namespace LibrarySystemApiV2.Dtos
{
    public class BookCopyDto
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public string BookTitle { get; set; } = string.Empty;
        public string BookNumber { get; set; } = string.Empty;
        public string Barcode { get; set; } = string.Empty;
        //public BookCopyStatus Status { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
