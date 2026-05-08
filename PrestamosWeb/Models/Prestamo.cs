namespace PrestamosWeb.Models
{
    public class Prestamo
    {
        public int IdPrestamo { get; set; }
        public int IdEmpleado { get; set; }
        public string NombreEmpleado { get; set; }
        public string NombreAutorizador { get; set; }
        public string NombreTipoPago { get; set; }

        public decimal CantidadTotalPrestada { get; set; }
        public decimal CantidadTotalAPagar { get; set; }

        public decimal InteresAprobado { get; set; }
        public decimal InteresMoratorio { get; set; }

        public int IdTipoPago { get; set; }

        public DateTime? FechaPrimerPago { get; set; }

        public decimal TotalAbonadoCapital { get; set; }
        public decimal TotalAbonadoIntereses { get; set; }

        public decimal Saldo { get; set; }

        public DateTime? FechaFinalPago { get; set; }

        public int IdAutoriza { get; set; }

        public string Notas { get; set; }

        public DateTime FechaRegistro { get; set; }
    }
}