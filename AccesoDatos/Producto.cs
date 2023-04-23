using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos
{
    public class Producto
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public decimal PrecioUnitario { get; set; }


        //Aqui se agrega en Existencias
        public void AgregarProducto(Producto producto)
        {
            try
            {
                string query = "INSERT INTO Existencias" +
                    "(Descripcion,PrecioUnitario) " +
                    "VALUES" +
                    "(@Descripcion,@PrecioUnitario)";

                using (SqlConnection con = new SqlConnection(query))
                {
                    SqlTransaction transaction = con.BeginTransaction();
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Transaction = transaction;

                        cmd.Parameters.AddWithValue("@Descripcion", producto.Descripcion);
                        cmd.Parameters.AddWithValue("@PrecioUnitario", producto.PrecioUnitario);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        //Actualiza el producto seleccionado mediante la Id
        public void ActualizarProducto(Producto producto)
        {
            try
            {
                string query = "UPDATE Existencias SET Descripcion = " +
                    "@Descripcion, PrecioUnitario = " +
                    "@PrecioUnitario WHERE Id = @Id"; //Aqui especifica la Id que cambiara

                using (SqlConnection con = new SqlConnection(query))
                {
                    SqlTransaction transaction = con.BeginTransaction();
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Transaction = transaction;

                        cmd.Parameters.AddWithValue("@Descripcion", producto.Descripcion);
                        cmd.Parameters.AddWithValue("@PrecioUnitario", producto.PrecioUnitario);
                        cmd.Parameters.AddWithValue("@Id", producto.Id);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        //Aqui elimina el producto que seleccionemos 
        public void EliminarProducto(int id) //Pongo el Id ya que se borrara mediante ella
        {
            try
            {
                string query = "DELETE FROM Existencias where Id = @Id";

                using (SqlConnection con = new SqlConnection(query))
                {
                    SqlTransaction transaction = con.BeginTransaction();
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Transaction = transaction;

                        cmd.Parameters.AddWithValue("@Id", id);

                        cmd.ExecuteNonQuery();
                    }
                }
            }

            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public Producto SeleccionarProducto (int id)
        {
            try
            {
                string query = "SELECT Id, Descripcion, PrecioUnitario FROM Existencias WHERE Id = @Id";

                using (SqlConnection con = new SqlConnection(query))
                {
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.CommandType = CommandType.Text;

                        cmd.Parameters.AddWithValue("@Id", id);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    Producto producto = new Producto();

                                    producto.Id = reader.GetInt32(0);
                                    producto.Descripcion = reader.GetString(1);
                                    producto.PrecioUnitario = reader.GetDecimal(2);

                                    return producto;
                                }
                            }
                        }
                    }
                }
                return null;
//No entiendo del todo, pero al agregar esto ya no da error en la Ln 117 "SeleccionarProducto" (Me spoileo el codigo)

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

    }
}
