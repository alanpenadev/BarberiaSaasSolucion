namespace BarberiaSaasApi.DTOs
{
    public class ServicioDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int DuracionMinutos { get; set; }
        public int BarberiaId { get; set; }
        public string BarberiaNombre { get; set; } = string.Empty;
    }

    public class ServicioCreateDto
    {
        public string Nombre { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int DuracionMinutos { get; set; }
        public int BarberiaId { get; set; }
    }
}