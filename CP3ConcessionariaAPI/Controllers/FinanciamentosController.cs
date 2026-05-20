using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CP3ConcessionariaAPI.Data;
using CP3ConcessionariaAPI.Models;
using CP3ConcessionariaAPI.Services;

namespace CP3ConcessionariaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FinanciamentosController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly FinanciamentoService _financiamentoService;

        public FinanciamentosController(AppDbContext context, FinanciamentoService financiamentoService)
        {
            _context = context;
            _financiamentoService = financiamentoService;
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
                return NotFound(new { mensagem = "Financiamento não encontrado." });

            return financiamento;
        }

        // POST: api/Financiamentos
        [HttpPost]
        public async Task<ActionResult<Financiamento>> PostFinanciamento(Financiamento financiamento)
        {
            // Calcula parcela automaticamente
            financiamento = _financiamentoService.PreencherFinanciamento(financiamento);
            var score = _financiamentoService.AvaliarScore(financiamento.ValorVeiculo, financiamento.ValorEntrada);

            if (score == "REPROVADO")
                return BadRequest(new { mensagem = "Financiamento reprovado. Entrada mínima de 10% do valor do veículo.", score });

            _context.Financiamentos.Add(financiamento);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFinanciamento), new { id = financiamento.IdProduto }, new
            {
                financiamento,
                score,
                mensagem = "Financiamento criado com sucesso."
            });
        }

        // PUT: api/Financiamentos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFinanciamento(int id, Financiamento financiamento)
        {
            if (id != financiamento.IdProduto)
                return BadRequest(new { mensagem = "ID da URL não confere com o ID do corpo." });

            financiamento = _financiamentoService.PreencherFinanciamento(financiamento);
            var score = _financiamentoService.AvaliarScore(financiamento.ValorVeiculo, financiamento.ValorEntrada);

            if (score == "REPROVADO")
                return BadRequest(new { mensagem = "Financiamento reprovado. Entrada mínima de 10% do valor do veículo.", score });

            _context.Entry(financiamento).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FinanciamentoExists(id))
                    return NotFound(new { mensagem = "Financiamento não encontrado." });
                else
                    throw;
            }

            return NoContent();
        }

        // DELETE: api/Financiamentos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFinanciamento(int id)
        {
            var financiamento = await _context.Financiamentos.FindAsync(id);

            if (financiamento == null)
                return NotFound(new { mensagem = "Financiamento não encontrado." });

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
