using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CP3ConcessionariaAPI.Data;
using CP3ConcessionariaAPI.Models;

namespace CP3ConcessionariaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConcessionariasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ConcessionariasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Concessionarias
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Concessionaria>>> GetConcessionarias()
        {
            return await _context.Concessionarias.ToListAsync();
        }

        // GET: api/Concessionarias/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Concessionaria>> GetConcessionaria(int id)
        {
            var concessionaria = await _context.Concessionarias.FindAsync(id);

            if (concessionaria == null)
            {
                return NotFound();
            }

            return concessionaria;
        }

        // PUT: api/Concessionarias/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutConcessionaria(int id, Concessionaria concessionaria)
        {
            if (id != concessionaria.IdConcessionaria)
            {
                return BadRequest();
            }

            _context.Entry(concessionaria).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ConcessionariaExists(id))
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

        // POST: api/Concessionarias
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Concessionaria>> PostConcessionaria(Concessionaria concessionaria)
        {
            _context.Concessionarias.Add(concessionaria);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetConcessionaria", new { id = concessionaria.IdConcessionaria }, concessionaria);
        }

        // DELETE: api/Concessionarias/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConcessionaria(int id)
        {
            var concessionaria = await _context.Concessionarias.FindAsync(id);
            if (concessionaria == null)
            {
                return NotFound();
            }

            _context.Concessionarias.Remove(concessionaria);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ConcessionariaExists(int id)
        {
            return _context.Concessionarias.Any(e => e.IdConcessionaria == id);
        }
    }
}
