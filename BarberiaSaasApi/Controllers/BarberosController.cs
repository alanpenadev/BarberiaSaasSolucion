using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BarberiaSaasApi.Core.Domain;
using BarberiaSaasApi.Infrastructure.Data;

namespace BarberiaSaasApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BarberosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BarberosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/barberos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Barbero>>> GetBarberos()
        {
            return await _context.Barberos.Include(b => b.Barberia).ToListAsync();
        }

        // GET: api/barberos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Barbero>> GetBarbero(int id)
        {
            var barbero = await _context.Barberos.Include(b => b.Barberia).FirstOrDefaultAsync(b => b.Id == id);

            if (barbero == null)
            {
                return NotFound();
            }

            return barbero;
        }

        // PUT: api/barberos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBarbero(int id, Barbero barbero)
        {
            if (id != barbero.Id)
            {
                return BadRequest();
            }

            _context.Entry(barbero).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BarberoExists(id))
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

        // POST: api/barberos
        [HttpPost]
        public async Task<ActionResult<Barbero>> PostBarbero(Barbero barbero)
        {
            _context.Barberos.Add(barbero);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBarbero), new { id = barbero.Id }, barbero);
        }

        // DELETE: api/barberos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBarbero(int id)
        {
            var barbero = await _context.Barberos.FindAsync(id);
            if (barbero == null)
            {
                return NotFound();
            }

            _context.Barberos.Remove(barbero);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BarberoExists(int id)
        {
            return _context.Barberos.Any(e => e.Id == id);
        }
    }
}