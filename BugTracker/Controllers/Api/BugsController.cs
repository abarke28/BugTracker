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
using System.Net.Http;

namespace BugTracker.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class BugsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public static string Endpoint { get; } = @"https://localhost:44313/api/bugs/";

        public BugsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Bugs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Bug>>> GetBug()
        {
            return await _context.Bugs.ToListAsync();
        }

        // GET: api/Bugs/{id}
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
            bug.DateSubmitted = DateTime.Now;
            bug.Status = BugStatus.Open;

            _context.Bugs.Add(bug);
            await _context.SaveChangesAsync();

            return Created(Request.Path.ToString() + "/" + bug.Id, bugDto);
        }

        // DELETE: api/Bugs/{id}
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
