using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CP3ConcessionariaAPI.Data;
using CP3ConcessionariaAPI.Models;

namespace CP3ConcessionariaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ClientesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Clientes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetClientes()
        {
            return await _context.Clientes
                .Include(c => c.Concessionaria)
                .ToListAsync();
        }

        // GET: api/Clientes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cliente>> GetCliente(int id)
        {
            var cliente = await _context.Clientes
                .Include(c => c.Concessionaria)
                .FirstOrDefaultAsync(c => c.IdCliente == id);

            if (cliente == null)
                return NotFound(new { mensagem = "Cliente não encontrado." });

            return cliente;
        }

        // POST: api/Clientes/pf
        [HttpPost("pf")]
        public async Task<ActionResult<PessoaFisica>> PostPessoaFisica(PessoaFisica pf)
        {
            // Verifica se concessionária existe
            var concessionaria = await _context.Concessionarias.FirstOrDefaultAsync(c => c.IdConcessionaria == pf.IdConcessionaria);
            if (concessionaria == null)
                return BadRequest(new { mensagem = "Concessionária informada não existe." });

            // Verifica CPF duplicado
            var cpfExistente = await _context.PessoasFisicas.FirstOrDefaultAsync(p => p.Cpf == pf.Cpf);
            if (cpfExistente != null)
                return BadRequest(new { mensagem = "CPF já cadastrado." });

            _context.PessoasFisicas.Add(pf);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCliente), new { id = pf.IdCliente }, pf);
        }

        // POST: api/Clientes/pj
        [HttpPost("pj")]
        public async Task<ActionResult<PessoaJuridica>> PostPessoaJuridica(PessoaJuridica pj)
        {
            // Verifica se concessionária existe
            var concessionaria = await _context.Concessionarias.FirstOrDefaultAsync(c => c.IdConcessionaria == pj.IdConcessionaria);
            if (concessionaria == null)
                return BadRequest(new { mensagem = "Concessionária informada não existe." });

            // Verifica CNPJ duplicado
            var cnpjExistente = await _context.PessoasJuridicas.FirstOrDefaultAsync(p => p.Cnpj == pj.Cnpj);
            if (cnpjExistente != null)
                return BadRequest(new { mensagem = "CNPJ já cadastrado." });

            _context.PessoasJuridicas.Add(pj);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCliente), new { id = pj.IdCliente }, pj);
        }

        // PUT: api/Clientes/pf/5
        [HttpPut("pf/{id}")]
        public async Task<IActionResult> PutPessoaFisica(int id, PessoaFisica pf)
        {
            if (id != pf.IdCliente)
                return BadRequest(new { mensagem = "ID da URL não confere com o ID do corpo." });

            var pfExistente = await _context.PessoasFisicas.FindAsync(id);
            if (pfExistente == null)
                return NotFound(new { mensagem = "Cliente PF não encontrado." });

            _context.Entry(pfExistente).CurrentValues.SetValues(pf);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PUT: api/Clientes/pj/5
        [HttpPut("pj/{id}")]
        public async Task<IActionResult> PutPessoaJuridica(int id, PessoaJuridica pj)
        {
            if (id != pj.IdCliente)
                return BadRequest(new { mensagem = "ID da URL não confere com o ID do corpo." });

            var pjExistente = await _context.PessoasJuridicas.FindAsync(id);
            if (pjExistente == null)
                return NotFound(new { mensagem = "Cliente PJ não encontrado." });

            _context.Entry(pjExistente).CurrentValues.SetValues(pj);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        // DELETE: api/Clientes/pf/5
        [HttpDelete("pf/{id}")]
        public async Task<IActionResult> DeletePessoaFisica(int id)
        {
            var pf = await _context.PessoasFisicas.FindAsync(id);

            if (pf == null)
                return NotFound(new { mensagem = "Cliente PF não encontrado." });

            _context.PessoasFisicas.Remove(pf);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Clientes/pj/5
        [HttpDelete("pj/{id}")]
        public async Task<IActionResult> DeletePessoaJuridica(int id)
        {
            var pj = await _context.PessoasJuridicas.FindAsync(id);

            if (pj == null)
                return NotFound(new { mensagem = "Cliente PJ não encontrado." });

            _context.PessoasJuridicas.Remove(pj);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ClienteExists(int id)
        {
            return _context.Clientes.Any(e => e.IdCliente == id);
        }
    }
}
