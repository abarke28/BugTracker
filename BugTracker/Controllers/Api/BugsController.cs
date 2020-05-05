﻿using System;
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

namespace BugTracker.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class BugsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public static string Endpoint { get; set; } = @"https://localhost:44313/api/bugs";

        public BugsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Bugs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BugDto>>> GetBug()
        {
            return await _context.Bug.Select(b=>_mapper.Map<BugDto>(b)).ToListAsync();
        }

        // GET: api/Bugs/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<BugDto>> GetBug(int id)
        {
            var bug = await _context.Bug.Include(b=>b.Comments).FirstOrDefaultAsync(b=>b.Id==id);

            if (bug == null)
            {
                return NotFound();
            }

            return _mapper.Map<BugDto>(bug);
        }

        // PUT: api/Bugs/{id}
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

        // POST: api/Bugs
        [HttpPost]
        public async Task<ActionResult<Bug>> PostBug(BugDto bugDto)
        {
            var bug = _mapper.Map<Bug>(bugDto);
            bug.DateSubmitted = DateTime.Today;
            bug.Status = BugStatus.Open;

            _context.Bug.Add(bug);
            await _context.SaveChangesAsync();

            return Created(Request.Path.ToString() + "/" + bug.Id, bugDto);
        }

        // DELETE: api/Bugs/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<Bug>> DeleteBug(int id)
        {
            var bug = await _context.Bug.FindAsync(id);
            if (bug == null)
            {
                return NotFound();
            }

            _context.Bug.Remove(bug);
            await _context.SaveChangesAsync();

            return bug;
        }

        private bool BugExists(int id)
        {
            return _context.Bug.Any(e => e.Id == id);
        }
    }
}
