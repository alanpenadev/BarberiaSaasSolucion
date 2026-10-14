using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BarberiaSaasApi.Core.Domain
{
    public class Barberia
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string Direccion { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Latitud { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Longitud { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.Now;
    }
}