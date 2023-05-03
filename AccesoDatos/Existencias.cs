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

        public SqlDataAdapter Obtener()
        {
            try
            {
                string query = "SELECT * FROM Existencias";
                SqlDataAdapter clientes = new SqlDataAdapter(query, Conexion.ConnectionString);

                return clientes;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Agregar(SqlConnection con, SqlTransaction transaction, int ProductoId)
        {
            string query = "Insert Into Existencias (Existencia, ProductoId) VALUES (0, @ProductoId)";

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Transaction = transaction;

                cmd.Parameters.AddWithValue("@ProductoId", ProductoId);
                cmd.ExecuteNonQuery();
            }
        }

        public void Actualizar(Existencias existencias)
        {
            try
            {
                string query = "UPDATE Existencias SET ProductoId, Existencias = " +
                    "@ProductoId, @Existencias";

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
    }
}
