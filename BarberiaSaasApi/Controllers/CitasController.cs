using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BarberiaSaasApi.Core.Domain;
using BarberiaSaasApi.Infrastructure.Data;
using BarberiaSaasApi.DTOs;

namespace BarberiaSaasApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CitasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/citas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CitaDto>>> GetCitas()
        {
            var citas = await _context.Citas
                .Include(c => c.Barberia)
                .Include(c => c.Barbero)
                .Include(c => c.Servicio)
                .Select(c => new CitaDto
                {
                    Id = c.Id,
                    FechaHora = c.FechaHora,
                    ClienteNombre = c.ClienteNombre,
                    BarberiaId = c.BarberiaId,
                    BarberiaNombre = c.Barberia != null ? c.Barberia.Nombre : string.Empty,
                    BarberoId = c.BarberoId,
                    BarberoNombre = c.Barbero != null ? c.Barbero.Nombre : string.Empty,
                    ServicioId = c.ServicioId,
                    ServicioNombre = c.Servicio != null ? c.Servicio.Nombre : string.Empty
                })
                .ToListAsync();

            return citas;
        }

        // GET: api/citas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CitaDto>> GetCita(int id)
        {
            var cita = await _context.Citas
                .Include(c => c.Barberia)
                .Include(c => c.Barbero)
                .Include(c => c.Servicio)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (cita == null)
            {
                return NotFound();
            }

            var citaDto = new CitaDto
            {
                Id = cita.Id,
                FechaHora = cita.FechaHora,
                ClienteNombre = cita.ClienteNombre,
                BarberiaId = cita.BarberiaId,
                BarberiaNombre = cita.Barberia != null ? cita.Barberia.Nombre : string.Empty,
                BarberoId = cita.BarberoId,
                BarberoNombre = cita.Barbero != null ? cita.Barbero.Nombre : string.Empty,
                ServicioId = cita.ServicioId,
                ServicioNombre = cita.Servicio != null ? cita.Servicio.Nombre : string.Empty
            };

            return citaDto;
        }

        // POST: api/citas
        [HttpPost]
        public async Task<ActionResult<CitaDto>> PostCita(CitaCreateDto createDto)
        {
            // Validar que la barbería exista
            var barberiaExists = await _context.Barberias.AnyAsync(b => b.Id == createDto.BarberiaId);
            if (!barberiaExists)
            {
                return BadRequest("La barbería especificada no existe.");
            }

            // Validar que el barbero pertenezca a esa barbería
            var barbero = await _context.Barberos.FirstOrDefaultAsync(b => b.Id == createDto.BarberoId && b.BarberiaId == createDto.BarberiaId);
            if (barbero == null)
            {
                return BadRequest("El barbero especificado no existe o no pertenece a esta barbería.");
            }

            // Validar que el servicio pertenezca a esa barbería
            var servicio = await _context.Servicios.FirstOrDefaultAsync(s => s.Id == createDto.ServicioId && s.BarberiaId == createDto.BarberiaId);
            if (servicio == null)
            {
                return BadRequest("El servicio especificado no existe o no pertenece a esta barbería.");
            }

            var cita = new Cita
            {
                FechaHora = createDto.FechaHora,
                ClienteNombre = createDto.ClienteNombre,
                BarberiaId = createDto.BarberiaId,
                BarberoId = createDto.BarberoId,
                ServicioId = createDto.ServicioId
            };

            _context.Citas.Add(cita);
            await _context.SaveChangesAsync();

            // Cargar relaciones para el DTO de respuesta
            await _context.Entry(cita).Reference(c => c.Barberia).LoadAsync();
            await _context.Entry(cita).Reference(c => c.Barbero).LoadAsync();
            await _context.Entry(cita).Reference(c => c.Servicio).LoadAsync();

            var citaDto = new CitaDto
            {
                Id = cita.Id,
                FechaHora = cita.FechaHora,
                ClienteNombre = cita.ClienteNombre,
                BarberiaId = cita.BarberiaId,
                BarberiaNombre = cita.Barberia?.Nombre ?? string.Empty,
                BarberoId = cita.BarberoId,
                BarberoNombre = cita.Barbero?.Nombre ?? string.Empty,
                ServicioId = cita.ServicioId,
                ServicioNombre = cita.Servicio?.Nombre ?? string.Empty
            };

            return CreatedAtAction(nameof(GetCita), new { id = cita.Id }, citaDto);
        }

        // PUT: api/citas/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCita(int id, CitaCreateDto updateDto)
        {
            var cita = await _context.Citas.FindAsync(id);
            if (cita == null)
            {
                return NotFound();
            }

            var barberiaExists = await _context.Barberias.AnyAsync(b => b.Id == updateDto.BarberiaId);
            if (!barberiaExists)
            {
                return BadRequest("La barbería especificada no existe.");
            }

            var barbero = await _context.Barberos.FirstOrDefaultAsync(b => b.Id == updateDto.BarberoId && b.BarberiaId == updateDto.BarberiaId);
            if (barbero == null)
            {
                return BadRequest("El barbero especificado no existe o no pertenece a esta barbería.");
            }

            var servicio = await _context.Servicios.FirstOrDefaultAsync(s => s.Id == updateDto.ServicioId && s.BarberiaId == updateDto.BarberiaId);
            if (servicio == null)
            {
                return BadRequest("El servicio especificado no existe o no pertenece a esta barbería.");
            }

            cita.FechaHora = updateDto.FechaHora;
            cita.ClienteNombre = updateDto.ClienteNombre;
            cita.BarberiaId = updateDto.BarberiaId;
            cita.BarberoId = updateDto.BarberoId;
            cita.ServicioId = updateDto.ServicioId;

            _context.Entry(cita).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CitaExists(id))
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

        // DELETE: api/citas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCita(int id)
        {
            var cita = await _context.Citas.FindAsync(id);
            if (cita == null)
            {
                return NotFound();
            }

            _context.Citas.Remove(cita);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CitaExists(int id)
        {
            return _context.Citas.Any(e => e.Id == id);
        }
    }
}