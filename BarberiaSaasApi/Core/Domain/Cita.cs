using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BarberiaSaasApi.Core.Domain
{
    public class Cita
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int BarberiaId { get; set; }
        [ForeignKey("BarberiaId")]
        public Barberia? Barberia { get; set; }

        [Required]
        public int BarberoId { get; set; }
        [ForeignKey("BarberoId")]
        public Barbero? Barbero { get; set; }

        [Required]
        public int ServicioId { get; set; }
        [ForeignKey("ServicioId")]
        public Servicio? Servicio { get; set; }

        [Required]
        [MaxLength(100)]
        public string ClienteNombre { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string ClienteTelefono { get; set; } = string.Empty;

        [Required]
        public DateTime FechaHora { get; set; }

        [Required]
        [MaxLength(50)]
        public string Estado { get; set; } = "Pendiente";

        public DateTime FechaCreacion { get; set; } = DateTime.Now;
    }
}