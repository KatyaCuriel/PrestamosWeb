namespace PrestamosWeb.Models
{

    public class PrestamoHistorico
    {
        public int IdHistorico { get; set; }
        public int IdPrestamo { get; set; }
        public string Accion { get; set; }
        public string Detalle { get; set; }
        public DateTime FechaMovimiento { get; set; }
    }
}
