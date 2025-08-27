using LibrarySystemApi.Data;
using LibrarySystemApi.Dtos;
using LibrarySystemApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace LibrarySystemApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController(BookLibraryDbContext context) : ControllerBase
    {
        private readonly BookLibraryDbContext _context = context;

        [HttpGet]
        public async Task<ActionResult<List<Member>>> GetMembers()
        {
            var members = await _context.Members.ToListAsync();
            return Ok(members);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult> GetMemberById(int id)
        {
            var existingMembers = await _context.Members.FindAsync(id);
            if (existingMembers == null) 
                return NotFound($"Member is not found");
            return Ok(existingMembers);
        }
        [HttpPost]
        public async Task<ActionResult> AddMembers(CreateMemberDto newMemberDto)
        {
            if (newMemberDto == null)
                return BadRequest();

            var newMember = new Member
            {
                FullName = newMemberDto.FullName,
                Email = newMemberDto.Email,
                JoinedDate = DateTime.UtcNow
            };

            _context.Members.Add(newMember);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetMemberById), new {id = newMember.Id}, newMember);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMember(int id, UpdateMemberDto updatedMemberDto)
        {
            var existingMember = await _context.Members.FindAsync(id);
            if (existingMember == null)
                return NotFound();
            existingMember.FullName = updatedMemberDto.FullName;
            existingMember.Email = updatedMemberDto.Email;
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMember(int id)
        {
            var existingMember = await _context.Members.FindAsync(id);
            if(existingMember == null)
                return NotFound();
            _context.Members.Remove(existingMember);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
