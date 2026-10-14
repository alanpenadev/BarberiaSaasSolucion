using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BarberiaSaasApi.Core.Domain;
using BarberiaSaasApi.Infrastructure.Data;

namespace BarberiaSaasApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BarberiasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BarberiasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/barberias
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Barberia>>> GetBarberias()
        {
            return await _context.Barberias.ToListAsync();
        }

        // GET: api/barberias/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Barberia>> GetBarberia(int id)
        {
            var barberia = await _context.Barberias.FindAsync(id);

            if (barberia == null)
            {
                return NotFound();
            }

            return barberia;
        }

        // PUT: api/barberias/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBarberia(int id, Barberia barberia)
        {
            if (id != barberia.Id)
            {
                return BadRequest();
            }

            _context.Entry(barberia).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BarberiaExists(id))
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

        // POST: api/barberias
        [HttpPost]
        public async Task<ActionResult<Barberia>> PostBarberia(Barberia barberia)
        {
            _context.Barberias.Add(barberia);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBarberia), new { id = barberia.Id }, barberia);
        }

        // DELETE: api/barberias/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBarberia(int id)
        {
            var barberia = await _context.Barberias.FindAsync(id);
            if (barberia == null)
            {
                return NotFound();
            }

            _context.Barberias.Remove(barberia);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BarberiaExists(int id)
        {
            return _context.Barberias.Any(e => e.Id == id);
        }
    }
}