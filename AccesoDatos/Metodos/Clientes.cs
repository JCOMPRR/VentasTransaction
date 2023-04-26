using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos
{
    public class Clientes
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        public void AgregarCliente(Clientes cliente)
        {
            try
            {
                string query = "INSERT INTO Clientes" +
                    "(Id, Nombre) " +
                    "VALUES" +
                    "(@Id,@Nombre)";
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public void ActualizarCliente(Clientes cliente)
        {
            try
            {
                string query = "UPDATE Clientes SET Id, Nombre = " +
                    "@Id, @Nombre";

                using (SqlConnection con = new SqlConnection(query))
                {
                    SqlTransaction transaction = con.BeginTransaction();
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Transaction = transaction;

                        cmd.Parameters.AddWithValue("@Id", cliente.Id);
                        cmd.Parameters.AddWithValue("@Nombre", cliente.Nombre);


                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public void EliminarCliente(int Id)
        {
            try
            {
                string query = "DELETE FROM Clientes where Id = @Id";

                using (SqlConnection con = new SqlConnection(query))
                {
                    SqlTransaction transaction = con.BeginTransaction();
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Transaction = transaction;

                        cmd.Parameters.AddWithValue("@Id", Id);

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
