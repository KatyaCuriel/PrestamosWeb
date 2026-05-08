using System.Data;
using System.Data.SqlClient;
using PrestamosWeb.Models;
using System.Collections.Generic;
using System;

namespace PrestamosWeb.Repositories
{
    public class PrestamoRepository
    {
        private readonly string _conexion;

        public PrestamoRepository(IConfiguration configuration)
        {
            _conexion = configuration.GetConnectionString("DefaultConnection");
        }

        public List<Prestamo> Listar()
        {
            List<Prestamo> lista = new List<Prestamo>();

            using (SqlConnection cn = new SqlConnection(_conexion))
            {
                SqlCommand cmd = new SqlCommand("sp_Prestamos_Listar", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cn.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    lista.Add(new Prestamo()
                    {
                        IdPrestamo = Convert.ToInt32(dr["IdPrestamo"]),
                        IdEmpleado = Convert.ToInt32(dr["IdEmpleado"]),
                        NombreEmpleado = dr["NombreEmpleado"].ToString(),
                        NombreAutorizador = dr["NombreAutorizador"].ToString(),
                        NombreTipoPago = dr["NombreTipoPago"].ToString(),
                        CantidadTotalPrestada = Convert.ToDecimal(dr["CantidadTotalPrestada"]),
                        CantidadTotalAPagar = Convert.ToDecimal(dr["CantidadTotalAPagar"]),
                        Saldo = Convert.ToDecimal(dr["Saldo"]),
                        Notas = dr["Notas"].ToString()
                    });
                }
            }

            return lista;
        }
        public void Insertar(Prestamo model)
        {
            using (SqlConnection cn = new SqlConnection(_conexion))
            {
                SqlCommand cmd = new SqlCommand("sp_Prestamos_Insertar", cn);

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@IdEmpleado", model.IdEmpleado);
                cmd.Parameters.AddWithValue("@CantidadTotalPrestada", model.CantidadTotalPrestada);
                cmd.Parameters.AddWithValue("@CantidadTotalAPagar", model.CantidadTotalAPagar);
                cmd.Parameters.AddWithValue("@InteresAprobado", model.InteresAprobado);
                cmd.Parameters.AddWithValue("@InteresMoratorio", model.InteresMoratorio);
                cmd.Parameters.AddWithValue("@IdTipoPago", model.IdTipoPago);
                cmd.Parameters.AddWithValue("@FechaPrimerPago", (object?)model.FechaPrimerPago ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@TotalAbonadoCapital", model.TotalAbonadoCapital);
                cmd.Parameters.AddWithValue("@TotalAbonadoIntereses", model.TotalAbonadoIntereses);
                cmd.Parameters.AddWithValue("@Saldo", model.Saldo);
                cmd.Parameters.AddWithValue("@FechaFinalPago", (object?)model.FechaFinalPago ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@IdAutoriza", model.IdAutoriza);
                cmd.Parameters.AddWithValue("@Notas", model.Notas);

                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public List<Empleado> ListarEmpleados()
        {
            List<Empleado> lista = new List<Empleado>();

            using (SqlConnection cn = new SqlConnection(_conexion))
            {
                SqlCommand cmd = new SqlCommand("SELECT IdEmpleado,Nombres,Apellido1,Apellido2 FROM Empleados", cn);

                cn.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    lista.Add(new Empleado()
                    {
                        IdEmpleado = Convert.ToInt32(dr["IdEmpleado"]),
                        Nombres = dr["Nombres"].ToString(),
                        Apellido1 = dr["Apellido1"].ToString(),
                        Apellido2 = dr["Apellido2"].ToString()
                    });
                }
            }

            return lista;
        }

        public Prestamo Buscar(int id)
        {
            Prestamo model = new Prestamo();

            using (SqlConnection cn = new SqlConnection(_conexion))
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT * FROM Prestamos WHERE IdPrestamo=@IdPrestamo", cn);

                cmd.Parameters.AddWithValue("@IdPrestamo", id);

                cn.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    model.IdPrestamo = Convert.ToInt32(dr["IdPrestamo"]);
                    model.IdEmpleado = Convert.ToInt32(dr["IdEmpleado"]);

                    model.CantidadTotalPrestada =
                        dr["CantidadTotalPrestada"] == DBNull.Value ? 0 :
                        Convert.ToDecimal(dr["CantidadTotalPrestada"]);

                    model.CantidadTotalAPagar =
                        dr["CantidadTotalAPagar"] == DBNull.Value ? 0 :
                        Convert.ToDecimal(dr["CantidadTotalAPagar"]);

                    model.InteresAprobado =
                        dr["InteresAprobado"] == DBNull.Value ? 0 :
                        Convert.ToDecimal(dr["InteresAprobado"]);

                    model.InteresMoratorio =
                        dr["InteresMoratorio"] == DBNull.Value ? 0 :
                        Convert.ToDecimal(dr["InteresMoratorio"]);

                    model.IdTipoPago =
                        dr["IdTipoPago"] == DBNull.Value ? 0 :
                        Convert.ToInt32(dr["IdTipoPago"]);

                    model.FechaPrimerPago =
                        dr["FechaPrimerPago"] == DBNull.Value ? null :
                        Convert.ToDateTime(dr["FechaPrimerPago"]);

                    model.TotalAbonadoCapital =
                        dr["TotalAbonadoCapital"] == DBNull.Value ? 0 :
                        Convert.ToDecimal(dr["TotalAbonadoCapital"]);

                    model.TotalAbonadoIntereses =
                        dr["TotalAbonadoIntereses"] == DBNull.Value ? 0 :
                        Convert.ToDecimal(dr["TotalAbonadoIntereses"]);

                    model.Saldo =
                        dr["Saldo"] == DBNull.Value ? 0 :
                        Convert.ToDecimal(dr["Saldo"]);

                    model.FechaFinalPago =
                        dr["FechaFinalPago"] == DBNull.Value ? null :
                        Convert.ToDateTime(dr["FechaFinalPago"]);

                    model.IdAutoriza =
                        dr["IdAutoriza"] == DBNull.Value ? 0 :
                        Convert.ToInt32(dr["IdAutoriza"]);

                    model.Notas = dr["Notas"].ToString();
                }
            }

            return model;
        }

        public void Actualizar(Prestamo model)

        {
            RecalcularPrestamo(model);

            using (SqlConnection cn = new SqlConnection(_conexion))
            {
                SqlCommand cmd = new SqlCommand("sp_Prestamos_Actualizar", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@IdPrestamo", model.IdPrestamo);
                cmd.Parameters.AddWithValue("@IdEmpleado", model.IdEmpleado);

                cmd.Parameters.AddWithValue("@CantidadTotalPrestada", model.CantidadTotalPrestada);
                cmd.Parameters.AddWithValue("@CantidadTotalAPagar", model.CantidadTotalAPagar);

                cmd.Parameters.AddWithValue("@InteresAprobado", model.InteresAprobado);
                cmd.Parameters.AddWithValue("@InteresMoratorio", model.InteresMoratorio);

                cmd.Parameters.AddWithValue("@IdTipoPago", model.IdTipoPago);

                cmd.Parameters.AddWithValue("@FechaPrimerPago",
                    (object?)model.FechaPrimerPago ?? DBNull.Value);

                cmd.Parameters.AddWithValue("@TotalAbonadoCapital", model.TotalAbonadoCapital);
                cmd.Parameters.AddWithValue("@TotalAbonadoIntereses", model.TotalAbonadoIntereses);

                cmd.Parameters.AddWithValue("@Saldo", model.Saldo);

                cmd.Parameters.AddWithValue("@FechaFinalPago",
                    (object?)model.FechaFinalPago ?? DBNull.Value);

                cmd.Parameters.AddWithValue("@IdAutoriza", model.IdAutoriza);

                cmd.Parameters.AddWithValue("@Notas", model.Notas ?? "");

                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public void Eliminar(int id)
        {
            using (SqlConnection cn = new SqlConnection(_conexion))
            {
                SqlCommand cmd = new SqlCommand("sp_Prestamos_Eliminar", cn);

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@IdPrestamo", id);

                cn.Open();

                cmd.ExecuteNonQuery();
            }
        }

        public List<TipoPago> ListarTiposPago()
        {
            List<TipoPago> lista = new List<TipoPago>();

            using (SqlConnection cn = new SqlConnection(_conexion))
            {
                SqlCommand cmd = new SqlCommand(
                "SELECT IdTipoPago, NombreCorto FROM TipoPagos", cn);

                cn.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    lista.Add(new TipoPago()
                    {
                        Id = Convert.ToInt32(dr["IdTipoPago"]),
                        NombreCorto = dr["NombreCorto"].ToString()
                    });
                }
            }

            return lista;
        }
        private void RecalcularPrestamo(Prestamo model)
        {
            // Total a pagar con interés
            model.CantidadTotalAPagar =
                model.CantidadTotalPrestada +
                (model.CantidadTotalPrestada * model.InteresAprobado);

            // Saldo automático
            model.Saldo =
                model.CantidadTotalAPagar -
                (model.TotalAbonadoCapital + model.TotalAbonadoIntereses);
         }

        public void RegistrarHistorico(int idPrestamo, string accion, string detalle)
        {
            using (SqlConnection cn = new SqlConnection(_conexion))
            {
                SqlCommand cmd = new SqlCommand(@"
            INSERT INTO PrestamosHistorico
            (IdPrestamo, Accion, Detalle, FechaMovimiento)
            VALUES
            (@IdPrestamo, @Accion, @Detalle, GETDATE())", cn);

                cmd.Parameters.AddWithValue("@IdPrestamo", idPrestamo);
                cmd.Parameters.AddWithValue("@Accion", accion);
                cmd.Parameters.AddWithValue("@Detalle", detalle ?? "");

                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public List<PrestamoHistorico> ListarHistorico(int idPrestamo)
        {
            List<PrestamoHistorico> lista = new List<PrestamoHistorico>();

            using (SqlConnection cn = new SqlConnection(_conexion))
            {
                SqlCommand cmd = new SqlCommand(@"
            SELECT *
            FROM PrestamosHistorico
            WHERE IdPrestamo = @IdPrestamo
            ORDER BY FechaMovimiento DESC", cn);

                cmd.Parameters.AddWithValue("@IdPrestamo", idPrestamo);

                cn.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    lista.Add(new PrestamoHistorico
                    {
                        IdHistorico = Convert.ToInt32(dr["IdHistorico"]),
                        IdPrestamo = Convert.ToInt32(dr["IdPrestamo"]),
                        Accion = dr["Accion"].ToString(),
                        Detalle = dr["Detalle"].ToString(),
                        FechaMovimiento = Convert.ToDateTime(dr["FechaMovimiento"])
                    });
                }
            }

            return lista;
        }
        public object ObtenerDashboard()
        {
            using (SqlConnection cn = new SqlConnection(_conexion))
            {
                SqlCommand cmd = new SqlCommand(@"
            SELECT 
                COUNT(*) AS TotalPrestamos,
                SUM(CantidadTotalPrestada) AS TotalPrestado,
                SUM(CantidadTotalAPagar) AS TotalPorCobrar,
                SUM(Saldo) AS TotalSaldo
            FROM Prestamos", cn);

                cn.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    return new
                    {
                        TotalPrestamos = Convert.ToInt32(dr["TotalPrestamos"]),
                        TotalPrestado = Convert.ToDecimal(dr["TotalPrestado"]),
                        TotalPorCobrar = Convert.ToDecimal(dr["TotalPorCobrar"]),
                        TotalSaldo = Convert.ToDecimal(dr["TotalSaldo"])
                    };
                }
            }

            return null;
        }
    }

}