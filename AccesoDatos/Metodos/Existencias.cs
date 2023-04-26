using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos
{
    public class Existencias
    {
        public int Id { get; set; }
        public int ProductoId{ get; set; }
        public int Existencia { get; set;}

        public void AgregarExistencia(Existencias existencias)
        {
            try
            {
                string query = "INSERT INTO Existencias" +
                    "(Id, ProductoId, Existencias)" +
                    "VALUES" +
                    "(@Id, @ProductoId, @Existencias)";
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public void ActualizarExistencia(Existencias existencias)
        {
            try
            {
                string query = "UPDATE Existencias SET Id, ProductoId, Existencias = " +
                    "@Id, @ProductoId, @Existencias";

                using (SqlConnection con = new SqlConnection(query))
                {
                    SqlTransaction transaction = con.BeginTransaction();
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Transaction = transaction;

                        cmd.Parameters.AddWithValue("@Id", existencias.Id);
                        cmd.Parameters.AddWithValue("@ProductoId", existencias.ProductoId);
                        cmd.Parameters.AddWithValue("@Existencias", existencias.Existencia);


                        cmd.ExecuteNonQuery();
                    }
                }     
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public void EliminarExistencia(int Existencia)
        {
            string query = "DELETE FROM Existencias WHERE Existencia = @Existencia";
        }
    }
}
