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
    public class FinanciamentosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FinanciamentosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Financiamentos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Financiamento>>> GetFinanciamentos()
        {
            return await _context.Financiamentos.ToListAsync();
        }

        // GET: api/Financiamentos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Financiamento>> GetFinanciamento(int id)
        {
            var financiamento = await _context.Financiamentos.FindAsync(id);

            if (financiamento == null)
            {
                return NotFound();
            }

            return financiamento;
        }

        // PUT: api/Financiamentos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFinanciamento(int id, Financiamento financiamento)
        {
            if (id != financiamento.IdProduto)
            {
                return BadRequest();
            }

            _context.Entry(financiamento).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FinanciamentoExists(id))
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

        // POST: api/Financiamentos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Financiamento>> PostFinanciamento(Financiamento financiamento)
        {
            _context.Financiamentos.Add(financiamento);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFinanciamento", new { id = financiamento.IdProduto }, financiamento);
        }

        // DELETE: api/Financiamentos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFinanciamento(int id)
        {
            var financiamento = await _context.Financiamentos.FindAsync(id);
            if (financiamento == null)
            {
                return NotFound();
            }

            _context.Financiamentos.Remove(financiamento);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FinanciamentoExists(int id)
        {
            return _context.Financiamentos.Any(e => e.IdProduto == id);
        }
    }
}
