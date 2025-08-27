using LibrarySystemApi.Models;

namespace LibrarySystemApi.Dtos
{
    public class LoanDto
    {
        public int Id { get; set; }
        public string BookName { get; set; } = string.Empty;
        public int BookCopyId{ get; set; }
        public string CopyNumber { get; set; } = string.Empty;
        public int MemberId { get; set; }
        public string Borrower { get; set; } = string.Empty;
        public DateTime LoanDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
    }
}
