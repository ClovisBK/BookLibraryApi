using LibrarySystemApiV2.Models;

namespace LibrarySystemApi.Models
{
    public class Loan
    {
        public int Id  { get; set; }
        public int BookCopyId { get; set; }
        public BookCopy? BookCopy { get; set; }
        public int MemberId { get; set; }
        public Member? Member { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
    }
}
