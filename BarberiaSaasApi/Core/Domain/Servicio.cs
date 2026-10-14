using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BarberiaSaasApi.Core.Domain
{
    public class Servicio
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Descripcion { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Precio { get; set; }

        [Required]
        public int DuracionMinutos { get; set; } = 30;

        [Required]
        public int BarberiaId { get; set; }

        [ForeignKey("BarberiaId")]
        public Barberia? Barberia { get; set; }

        public bool Activo { get; set; } = true;
    }
}