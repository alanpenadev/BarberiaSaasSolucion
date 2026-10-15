namespace BarberiaSaasApi.DTOs
{
    public class BarberoDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Especialidad { get; set; } = string.Empty;
        public int BarberiaId { get; set; }
        public string BarberiaNombre { get; set; } = string.Empty; // Útil para mostrar a qué barbería pertenece
    }

    public class BarberoCreateDto
    {
        public string Nombre { get; set; } = string.Empty;
        public string Especialidad { get; set; } = string.Empty;
        public int BarberiaId { get; set; }
    }
}