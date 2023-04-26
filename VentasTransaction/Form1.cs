using AccesoDatos;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace VentasTransaction
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        //"referencia de objeto no establecida en una instancia de un objeto"
        //Como chuchas arreglo esto???
        private void button1_Click(object sender, EventArgs e)
        {
            GuardarVenta();
        }

        //Debemos reubicar este metodo 
        private void GuardarVenta()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Conexion.ConnectionString))
                {
                    SqlTransaction transaction;
                    con.Open();
                    //Mantener la transaccion
                    transaction = con.BeginTransaction();

                    try
                    {
                        //Debe iniciar con 0
                        string query = "select top(1) Folio from Folios";
                        int folioActual = 0;
                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Transaction = transaction;

                            if (!int.TryParse(cmd.ExecuteScalar().ToString(), out folioActual))
                            {
                                throw new Exception("Ocurrio un error al obtener el folio");
                            }
                        }

                        Venta venta = new Venta();
                        venta.CLienteId = 1;
                        venta.Folio = folioActual + 1;
                        venta.Fecha = DateTime.Now;

                        VentaDetalle producto1 = new VentaDetalle();
                        producto1.ProductoId = 1;
                        producto1.Cantidad = 1;
                        producto1.Descripcion = "Azucar kg";
                        producto1.PrecioUnitario = 27.00m;
                        producto1.Importe = producto1.Cantidad * producto1.PrecioUnitario;

                        VentaDetalle producto2 = new VentaDetalle();
                        producto2.ProductoId = 2;
                        producto2.Cantidad = 1;
                        producto2.Descripcion = "Jugo Mango";
                        producto2.PrecioUnitario = 10.00m;
                        producto2.Importe = producto2.Cantidad * producto2.PrecioUnitario;

                        venta.Conceptos.Add(producto1);
                        venta.Conceptos.Add(producto2);

                        query = "INSERT INTO Ventas " +
                            "(Folio,Fecha,ClienteId,Total) " +
                            "VALUES " +
                            "(@Folio,@Fecha,@ClienteId,@Total);select scope_identity()";

                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Transaction = transaction;
                            cmd.Parameters.AddWithValue("@Folio", venta.Folio);
                            cmd.Parameters.AddWithValue("@Fecha", venta.Fecha);
                            cmd.Parameters.AddWithValue("@ClienteId", venta.CLienteId);
                            cmd.Parameters.AddWithValue("@Total", venta.Total);
                            

                            if (!int.TryParse(cmd.ExecuteScalar().ToString(), out int idVenta))
                            {
                                throw new Exception("Ocurrio un error al obtener el id de la venta");
                            }
                            venta.Id = idVenta;
                        }

                        foreach (VentaDetalle concepto in venta.Conceptos) 
                        {

                            query = "INSERT INTO VentasDetalle" +
                                    "(VentaId,ProductoId,Cantidad,Descripcion,PrecioUnitario,Importe) " +
                                    "VALUES" +
                                    "(@VentaId,@ProductoId,@Cantidad,@Descripcion,@PrecioUnitario,@Importe)";

                            using (SqlCommand cmd = new SqlCommand(query, con)) 
                            {
                                cmd.CommandType = CommandType.Text;
                                cmd.Transaction = transaction;

                                cmd.Parameters.AddWithValue("@VentaId", venta.Id);
                                cmd.Parameters.AddWithValue("@ProductoId", concepto.ProductoId);
                                cmd.Parameters.AddWithValue("@Cantidad", concepto.Cantidad);
                                cmd.Parameters.AddWithValue("@Descripcion", concepto.Descripcion);
                                cmd.Parameters.AddWithValue("@PrecioUnitario", concepto.PrecioUnitario);
                                cmd.Parameters.AddWithValue("@Importe", concepto.Importe);
                                cmd.ExecuteNonQuery();
                            }


                            query = "Update Existencias " +
                                    "set Existencia = Existencia-@Cantidad " +
                                    "where ProductoId = @ProductoId";

                            using (SqlCommand cmd = new SqlCommand(query, con))
                            {
                                cmd.CommandType = CommandType.Text;
                                cmd.Transaction = transaction;

                                cmd.Parameters.AddWithValue("@ProductoId", concepto.ProductoId);
                                cmd.Parameters.AddWithValue("@Cantidad", concepto.Cantidad);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        query = "Update Folios set Folio = Folio + 1 ";

                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Transaction = transaction;
                            
                            cmd.ExecuteNonQuery();
                        }

                        transaction.Commit();

                        MessageBox.Show($"Venta guardada correctamente con folio {venta.Folio}");

                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception(ex.Message);
                    }
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show($"Ocurrio un error al guardar la venta {ex.Message}");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: esta línea de código carga datos en la tabla 'ventasTransactionDBDataSet1.Clientes' Puede moverla o quitarla según sea necesario.
            this.clientesTableAdapter.Fill(this.ventasTransactionDBDataSet1.Clientes);
            // TODO: esta línea de código carga datos en la tabla 'ventasTransactionDBDataSet.Productos' Puede moverla o quitarla según sea necesario.
            this.productosTableAdapter.Fill(this.ventasTransactionDBDataSet.Productos);

        }


        //No se que agregar aqui :(
        //DataGrid Productos
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        //DataGrid de Clientes
        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }


        //Botones de Productos
        private void button8_Click(object sender, EventArgs e)
        {
            AgregarProducto();
        }

        private void AgregarProducto()
        {
            throw new NotImplementedException();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ActualizarProducto();
        }

        private void ActualizarProducto()
        {
            throw new NotImplementedException();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            EliminarProducto();
        }

        private void EliminarProducto()
        {
            throw new NotImplementedException();
        }


        //Botones de Clientes
        private void button3_Click(object sender, EventArgs e)
        {
            AgregarCliente();
        }

        private void AgregarCliente()
        {
            throw new NotImplementedException();
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            ActualizarCliente();
        }

        private void ActualizarCliente()
        {
            throw new NotImplementedException();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            EliminarCliente();
        }

        private void EliminarCliente()
        {
            throw new NotImplementedException();
        }
    }
}
