using LibrarySystemApi.Data;
using LibrarySystemApi.Dtos;
using LibrarySystemApi.Models;
using LibrarySystemApiV2.Dtos;
using LibrarySystemApiV2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystemApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoanController : ControllerBase
    {
        private readonly BookLibraryDbContext _context;
        public LoanController(BookLibraryDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<List<LoanDto>>> GetLoand()
        {
            var loan = await _context.Loans
                .Include(b => b.BookCopy)
                    .ThenInclude(bc => bc!.Book)
                .Include(m => m.Member)
                .Select(b => new LoanDto
                {
                    Id = b.Id,
                   BookName = b.BookCopy.Book.Title,
                   Borrower = b.Member!.FullName,
                   CopyNumber = b.BookCopy!.BookNumber,
                   MemberId = b.MemberId,
                   LoanDate = b.LoanDate,
                   DueDate = b.DueDate,
                   ReturnDate =b.ReturnDate,

                }).ToListAsync();
            return Ok(loan);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<LoanDto>> GetLoanById(int id)
        {
            var existingLoan = await _context.Loans
                .Where(b => b.Id == id)
                .Include(b => b.BookCopy)
                 .ThenInclude(bc => bc!.Book)
                 .Include(l => l.Member)
                .Select(b => new LoanDto
                {
                    Id = b.Id,
                    BookName = b.BookCopy.Book.Title,
                    MemberId= b.MemberId,
                    Borrower = b.Member!.FullName,
                    CopyNumber = b.BookCopy!.BookNumber,
                    LoanDate= b.LoanDate,
                    DueDate = b.DueDate,
                    ReturnDate = b.ReturnDate

                }).FirstOrDefaultAsync();
            if(existingLoan == null)
            {
                return NotFound();
            }
            return Ok(existingLoan);
        }
        [HttpPost]
        public async Task<ActionResult> AddLoan(CreateLoanDto newLoanDto)
        {
            if (newLoanDto == null)
                return BadRequest();
            var bookCopy = await _context.BookCopies.FindAsync(newLoanDto.BookCopyId);
            if (bookCopy == null)
                return NotFound("Book copy not found");
            if(bookCopy.Status != BookCopyStatus.Available)
                return BadRequest($"This book is not available. Corrent status: {bookCopy.Status}");

            bookCopy.Status = BookCopyStatus.Loaned;


            var loan = new Loan
            {
                MemberId = newLoanDto.MemberId,
                BookCopyId = newLoanDto.BookCopyId,
                LoanDate = DateTime.UtcNow,
                DueDate= newLoanDto.DueDate
            };
            _context.Loans.Add(loan);
           
            await _context.SaveChangesAsync();
            //returning the create loan to the dto
            var loanDto = new LoanDto
            {
                Id = loan.Id,
                BookCopyId = loan.BookCopyId,
                MemberId = loan.MemberId,
                LoanDate = loan.LoanDate,
                DueDate = loan.DueDate,
                
            };
            return CreatedAtAction(nameof(GetLoanById), new {id = loan.Id}, loanDto);
           
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLoan(int id, CreateLoanDto updatedLoandDto)
        {
            var existingLoan = await _context.Loans.FindAsync(id);
            if (existingLoan == null)
                return NotFound();
            existingLoan.MemberId = updatedLoandDto.MemberId;
            existingLoan.BookCopyId = updatedLoandDto.BookCopyId;
            existingLoan.DueDate = updatedLoandDto.DueDate;
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLoan(int id)
        {
            var existingLoan = await _context.Loans.FindAsync();
            if(existingLoan == null)
                return NotFound();
            _context.Loans.Remove(existingLoan);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        [HttpPost("return")]
        public async Task<IActionResult> ReturnBook([FromBody] ReturnBookDto  returnDto)
        {
            var loan = await _context.Loans
                .Include(l => l.BookCopy)
                .FirstOrDefaultAsync(l => l.Id == returnDto.LoanId);
            if(loan == null)
                return NotFound("Loan record not found");
            if (loan.ReturnDate != null)
                return BadRequest("This book has been already returned.");
            if (loan.BookCopy == null)
                return NotFound("Associated book copy not found.");

            //updating the return Date as current date
            loan.ReturnDate = DateTime.UtcNow;
            loan.BookCopy.Status = BookCopyStatus.Available;

            await _context.SaveChangesAsync();
            return Ok(new {message = "Book returned successfully.", loanId = loan.Id});

        }
    }
}
