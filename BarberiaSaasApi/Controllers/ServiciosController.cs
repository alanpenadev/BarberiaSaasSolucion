using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BarberiaSaasApi.Core.Domain;
using BarberiaSaasApi.Infrastructure.Data;
using BarberiaSaasApi.DTOs;

namespace BarberiaSaasApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiciosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ServiciosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/servicios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServicioDto>>> GetServicios()
        {
            var servicios = await _context.Servicios
                .Include(s => s.Barberia)
                .Select(s => new ServicioDto
                {
                    Id = s.Id,
                    Nombre = s.Nombre,
                    Precio = s.Precio,
                    DuracionMinutos = s.DuracionMinutos,
                    BarberiaId = s.BarberiaId,
                    BarberiaNombre = s.Barberia != null ? s.Barberia.Nombre : string.Empty
                })
                .ToListAsync();

            return servicios;
        }

        // GET: api/servicios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ServicioDto>> GetServicio(int id)
        {
            var servicio = await _context.Servicios
                .Include(s => s.Barberia)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (servicio == null)
            {
                return NotFound();
            }

            var servicioDto = new ServicioDto
            {
                Id = servicio.Id,
                Nombre = servicio.Nombre,
                Precio = servicio.Precio,
                DuracionMinutos = servicio.DuracionMinutos,
                BarberiaId = servicio.BarberiaId,
                BarberiaNombre = servicio.Barberia != null ? servicio.Barberia.Nombre : string.Empty
            };

            return servicioDto;
        }

        // POST: api/servicios
        [HttpPost]
        public async Task<ActionResult<ServicioDto>> PostServicio(ServicioCreateDto createDto)
        {
            var barberiaExists = await _context.Barberias.AnyAsync(b => b.Id == createDto.BarberiaId);
            if (!barberiaExists)
            {
                return BadRequest("La barbería especificada no existe.");
            }

            var servicio = new Servicio
            {
                Nombre = createDto.Nombre,
                Precio = createDto.Precio,
                DuracionMinutos = createDto.DuracionMinutos,
                BarberiaId = createDto.BarberiaId
            };

            _context.Servicios.Add(servicio);
            await _context.SaveChangesAsync();

            await _context.Entry(servicio).Reference(s => s.Barberia).LoadAsync();

            var servicioDto = new ServicioDto
            {
                Id = servicio.Id,
                Nombre = servicio.Nombre,
                Precio = servicio.Precio,
                DuracionMinutos = servicio.DuracionMinutos,
                BarberiaId = servicio.BarberiaId,
                BarberiaNombre = servicio.Barberia?.Nombre ?? string.Empty
            };

            return CreatedAtAction(nameof(GetServicio), new { id = servicio.Id }, servicioDto);
        }

        // PUT: api/servicios/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutServicio(int id, ServicioCreateDto updateDto)
        {
            var servicio = await _context.Servicios.FindAsync(id);
            if (servicio == null)
            {
                return NotFound();
            }

            var barberiaExists = await _context.Barberias.AnyAsync(b => b.Id == updateDto.BarberiaId);
            if (!barberiaExists)
            {
                return BadRequest("La barbería especificada no existe.");
            }

            servicio.Nombre = updateDto.Nombre;
            servicio.Precio = updateDto.Precio;
            servicio.DuracionMinutos = updateDto.DuracionMinutos;
            servicio.BarberiaId = updateDto.BarberiaId;

            _context.Entry(servicio).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServicioExists(id))
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

        // DELETE: api/servicios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteServicio(int id)
        {
            var servicio = await _context.Servicios.FindAsync(id);
            if (servicio == null)
            {
                return NotFound();
            }

            _context.Servicios.Remove(servicio);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ServicioExists(int id)
        {
            return _context.Servicios.Any(e => e.Id == id);
        }
    }
}