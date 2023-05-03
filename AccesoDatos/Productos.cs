using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos
{
    public class Productos
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public decimal PrecioUnitario { get; set; }

        public void AgregarProducto(Productos producto)
        {
            try
            {
                string query = "INSERT INTO Productos" +
                    "(Id, Descripcion, PrecioUnitario) " +
                    "VALUES" +
                    "(@Id,@Descripcion,@PrecioUnitario)";

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
                    con.Close();
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        //Actualiza el producto seleccionado mediante la Id

        //En productos... al actualizar el producto estas usando una transaction también...
        //pero no está ni el rollback ni el commit, en este caso, solo se afecta una tabla, debe tener transaction?
        public void ActualizarProducto(Productos producto)
        {
            try
            {
                string query = "UPDATE Productos SET Descripcion, PrecioUnitario = " +
                    "@Descripcion, @PrecioUnitario";

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

        //Al eliminar un producto, no te va a permitir pues tiene una relación con la tabla de
        //existencias, si tiene la transaction, si vas afectar existencias y productos,
        //es correcto pero no veo el llamado a eliminar existencias ni tampoco el commit ni rollback

        public void EliminarProducto(int id) //Pongo el Id ya que se borrara mediante ella
        {
            try
            {
                string query = "DELETE FROM Productos where Id = @Id";

                using (SqlConnection con = new SqlConnection(query))
                {
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.CommandType = CommandType.Text;

                        cmd.Parameters.AddWithValue("@Id", id);

                        cmd.ExecuteNonQuery();
                    }
                    con.Close();
                }
            }

            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
