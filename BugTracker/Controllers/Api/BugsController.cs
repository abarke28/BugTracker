using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BugTracker.Data;
using BugTracker.Models;
using AutoMapper;
using BugTracker.Models.Dtos;
using System.Net.Http;

namespace BugTracker.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class BugsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public BugsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Bug>>> GetBug()
        {
            return await _context.Bugs.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Bug>> GetBug(int id)
        {
            var bug = await _context.Bugs.Include(b=>b.Comments).FirstOrDefaultAsync(b=>b.Id==id);

            if (bug == null)
            {
                return NotFound();
            }

            return bug;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutBug(int id, Bug bug)
        {
            if (id != bug.Id)
            {
                return BadRequest();
            }

            _context.Entry(bug).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BugExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Bug>> PostBug(BugDto bugDto)
        {
            var bug = _mapper.Map<Bug>(bugDto);
            bug.DateSubmitted = DateTime.Now;
            bug.DateTargeted = DateTime.Now + TimeSpan.FromDays(14); // Default target of two weeks.
            bug.Status = BugStatus.Open;

            _context.Bugs.Add(bug);
            await _context.SaveChangesAsync();

            return Created(Request.Path.ToString() + "/" + bug.Id, bug);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Bug>> DeleteBug(int id)
        {
            var bug = await _context.Bugs.FindAsync(id);
            if (bug == null)
            {
                return NotFound();
            }

            _context.Bugs.Remove(bug);
            await _context.SaveChangesAsync();

            return bug;
        }

        private bool BugExists(int id)
        {
            return _context.Bugs.Any(e => e.Id == id);
        }
    }
}
