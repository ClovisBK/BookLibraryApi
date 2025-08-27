using LibrarySystemApi.Data;
using LibrarySystemApiV2.Dtos;
using LibrarySystemApiV2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystemApiV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookCopiesController(BookLibraryDbContext context) : ControllerBase
    {
        private readonly BookLibraryDbContext _context = context;


        [HttpGet]
        public async Task<ActionResult<List<BookCopyDto>>> GetBookCopies()
        {
            var copy = await _context.BookCopies
                .Select(c => new BookCopyDto
                {
                    Id = c.Id,
                    BookId = c.BookId,
                    BookTitle = c.Book!.Title,
                    BookNumber = c.BookNumber,
                    Barcode = c.Barcode,
                    Status = c.Status.ToString()
                }).ToListAsync();
            return Ok(copy);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<BookCopy>> GetBookCopyById(int id)
        {
            var existingBookCopy = await _context.BookCopies
                .Where(b => b.Id == id)
                .Include(b => b.Book)
                .Select(b => new BookCopyDto
                {
                    Id = b.Id,
                    BookId = b.BookId,
                    BookTitle = b.Book.Title,
                    BookNumber= b.BookNumber,
                    Barcode= b.Barcode,
                    Status = b.Status.ToString()
                })
                .FirstOrDefaultAsync();
            if (existingBookCopy is null)
            {
                return NotFound();
            }
            return Ok(existingBookCopy);
        }
        [HttpPost]
        public async Task<ActionResult> AddBookCopy(CreateBookCopyDto bookCopyDto)
        {
            if (bookCopyDto == null)
                return BadRequest();
            var bookCopy = new BookCopy
            {
                BookId = bookCopyDto.BookId,
                BookNumber = bookCopyDto.BookNumber,
                Barcode = bookCopyDto.Barcode,
                Status = BookCopyStatus.Available
            };
            _context.BookCopies.Add(bookCopy);
            await _context.SaveChangesAsync();
            //return the created BookCopy to the dto
            var bookdto = new BookCopyDto
            {
                Id = bookCopy.Id,
                BookId = bookCopy.BookId,
                BookNumber= bookCopy.BookNumber,
                Barcode = bookCopy.Barcode
            };
            return Ok(bookdto);

        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBookCopy(int id, CreateBookCopyDto UpdatebookCopy)
        {
            var  existingCopy = await _context.BookCopies.FindAsync(id);
            if (existingCopy is null)
                return NotFound("This book copy is not found");
            existingCopy.BookId = UpdatebookCopy.BookId;
            existingCopy.BookNumber = UpdatebookCopy.BookNumber;
            existingCopy.Barcode = UpdatebookCopy.Barcode;
            await _context.SaveChangesAsync();
            return NoContent();

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBookCopy(int id)
        {
            var existingCopy = await _context.BookCopies.FindAsync(id);
            if (existingCopy is null)
                return NotFound();
            _context.BookCopies.Remove(existingCopy);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
   
}
