namespace BarberiaSaasApi.DTOs
{
    // DTO para cuando el cliente solicita ver una barbería
    public class BarberiaDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
    }

    // DTO para cuando el cliente va a crear una nueva barbería (sin enviar el ID)
    public class BarberiaCreateDto
    {
        public string Nombre { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
    }
}