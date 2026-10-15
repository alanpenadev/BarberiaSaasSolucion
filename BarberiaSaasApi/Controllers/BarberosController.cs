using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BarberiaSaasApi.Core.Domain;
using BarberiaSaasApi.Infrastructure.Data;
using BarberiaSaasApi.DTOs;

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
        public async Task<ActionResult<IEnumerable<BarberoDto>>> GetBarberos()
        {
            var barberos = await _context.Barberos
                .Include(b => b.Barberia)
                .Select(b => new BarberoDto
                {
                    Id = b.Id,
                    Nombre = b.Nombre,
                    Especialidad = b.Especialidad,
                    BarberiaId = b.BarberiaId,
                    BarberiaNombre = b.Barberia != null ? b.Barberia.Nombre : string.Empty
                })
                .ToListAsync();

            return barberos;
        }

        // GET: api/barberos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BarberoDto>> GetBarbero(int id)
        {
            var barbero = await _context.Barberos
                .Include(b => b.Barberia)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (barbero == null)
            {
                return NotFound();
            }

            var barberoDto = new BarberoDto
            {
                Id = barbero.Id,
                Nombre = barbero.Nombre,
                Especialidad = barbero.Especialidad,
                BarberiaId = barbero.BarberiaId,
                BarberiaNombre = barbero.Barberia != null ? barbero.Barberia.Nombre : string.Empty
            };

            return barberoDto;
        }

        // POST: api/barberos
        [HttpPost]
        public async Task<ActionResult<BarberoDto>> PostBarbero(BarberoCreateDto createDto)
        {
            // Validamos que la barbería exista antes de registrar al barbero
            var barberiaExists = await _context.Barberias.AnyAsync(b => b.Id == createDto.BarberiaId);
            if (!barberiaExists)
            {
                return BadRequest("La barbería especificada no existe.");
            }

            var barbero = new Barbero
            {
                Nombre = createDto.Nombre,
                Especialidad = createDto.Especialidad,
                BarberiaId = createDto.BarberiaId
            };

            _context.Barberos.Add(barbero);
            await _context.SaveChangesAsync();

            // Cargamos la relación para devolver el nombre de la barbería en el DTO de respuesta
            await _context.Entry(barbero).Reference(b => b.Barberia).LoadAsync();

            var barberoDto = new BarberoDto
            {
                Id = barbero.Id,
                Nombre = barbero.Nombre,
                Especialidad = barbero.Especialidad,
                BarberiaId = barbero.BarberiaId,
                BarberiaNombre = barbero.Barberia?.Nombre ?? string.Empty
            };

            return CreatedAtAction(nameof(GetBarbero), new { id = barbero.Id }, barberoDto);
        }

        // PUT: api/barberos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBarbero(int id, BarberoCreateDto updateDto)
        {
            var barbero = await _context.Barberos.FindAsync(id);
            if (barbero == null)
            {
                return NotFound();
            }

            var barberiaExists = await _context.Barberias.AnyAsync(b => b.Id == updateDto.BarberiaId);
            if (!barberiaExists)
            {
                return BadRequest("La barbería especificada no existe.");
            }

            barbero.Nombre = updateDto.Nombre;
            barbero.Especialidad = updateDto.Especialidad;
            barbero.BarberiaId = updateDto.BarberiaId;

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