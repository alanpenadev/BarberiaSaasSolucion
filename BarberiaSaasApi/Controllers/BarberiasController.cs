using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BarberiaSaasApi.Core.Domain;
using BarberiaSaasApi.Infrastructure.Data;
using BarberiaSaasApi.DTOs;

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
        public async Task<ActionResult<IEnumerable<BarberiaDto>>> GetBarberias()
        {
            var barberias = await _context.Barberias
                .Select(b => new BarberiaDto
                {
                    Id = b.Id,
                    Nombre = b.Nombre,
                    Direccion = b.Direccion
                })
                .ToListAsync();

            return barberias;
        }

        // GET: api/barberias/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BarberiaDto>> GetBarberia(int id)
        {
            var barberia = await _context.Barberias.FindAsync(id);

            if (barberia == null)
            {
                return NotFound();
            }

            var barberiaDto = new BarberiaDto
            {
                Id = barberia.Id,
                Nombre = barberia.Nombre,
                Direccion = barberia.Direccion
            };

            return barberiaDto;
        }

        // POST: api/barberias
        [HttpPost]
        public async Task<ActionResult<BarberiaDto>> PostBarberia(BarberiaCreateDto createDto)
        {
            var barberia = new Barberia
            {
                Nombre = createDto.Nombre,
                Direccion = createDto.Direccion
            };

            _context.Barberias.Add(barberia);
            await _context.SaveChangesAsync();

            var barberiaDto = new BarberiaDto
            {
                Id = barberia.Id,
                Nombre = barberia.Nombre,
                Direccion = barberia.Direccion
            };

            return CreatedAtAction(nameof(GetBarberia), new { id = barberia.Id }, barberiaDto);
        }

        // PUT: api/barberias/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBarberia(int id, BarberiaCreateDto updateDto)
        {
            var barberia = await _context.Barberias.FindAsync(id);
            if (barberia == null)
            {
                return NotFound();
            }

            barberia.Nombre = updateDto.Nombre;
            barberia.Direccion = updateDto.Direccion;

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