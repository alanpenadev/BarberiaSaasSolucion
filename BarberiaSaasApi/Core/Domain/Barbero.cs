using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BarberiaSaasApi.Core.Domain
{
    public class Barbero
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Apellido { get; set; } = string.Empty;

        [MaxLength(150)]
        public string Especialidad { get; set; } = string.Empty;

        [Required]
        public int BarberiaId { get; set; }

        [ForeignKey("BarberiaId")]
        public Barberia? Barberia { get; set; }

        public bool Activo { get; set; } = true;
    }
}