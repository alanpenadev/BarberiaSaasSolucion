namespace BarberiaSaasApi.DTOs
{
    public class CitaDto
    {
        public int Id { get; set; }
        public DateTime FechaHora { get; set; }
        public string ClienteNombre { get; set; } = string.Empty;

        // Relaciones expandidas para que el cliente vea los nombres directamente en el JSON
        public int BarberiaId { get; set; }
        public string BarberiaNombre { get; set; } = string.Empty;

        public int BarberoId { get; set; }
        public string BarberoNombre { get; set; } = string.Empty;

        public int ServicioId { get; set; }
        public string ServicioNombre { get; set; } = string.Empty;
    }

    public class CitaCreateDto
    {
        public DateTime FechaHora { get; set; }
        public string ClienteNombre { get; set; } = string.Empty;
        public int BarberiaId { get; set; }
        public int BarberoId { get; set; }
        public int ServicioId { get; set; }
    }
}