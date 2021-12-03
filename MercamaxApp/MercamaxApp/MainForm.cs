using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace MercamaxApp
{
    public partial class MainForm : Form
    {
        /// <summary>
        /// Conexión a la base de datos
        /// </summary>
        public static SqlConnection con = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=DBMercaMx;Integrated Security=True");

        /// <summary>
        /// cc del usuario conectado
        /// </summary>
        public static int cc;

        /// <summary>
        /// indica si es empleado o no
        /// </summary>
        public static bool isEmploy;

        /// <summary>
        /// indica si es administrador o no
        /// </summary>
        public static bool isAdmin;

        /// <summary>
        /// Form de autenticación de usuario
        /// </summary>
        public LoginForm loginForm;

        /// <summary>
        /// nombre del usuario
        /// </summary>
        public static string nombre;
        
        /// <summary>
        /// DataTable de uso general
        /// </summary>
        public DataTable dt;

        /// <summary>
        /// DataTable de uso general
        /// </summary>
        public DataTable dt2;

        public MainForm()
        {
            InitializeComponent();            
        }

        public void BindFacturas()
        {
            dataGridView3.DataSource = null;
            dataGridView3.Columns.Clear();

            MainForm.con.Open();
            using (SqlCommand cmd = new SqlCommand("GetFacturas", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@id", cc));
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    dt2 = new DataTable();
                    sda.Fill(dt2);
                    dataGridView3.DataSource = dt2;
                }
            }
            MainForm.con.Close();

            dataGridView3.Columns[0].HeaderText = "Id Factura";
            dataGridView3.Columns[0].ReadOnly = true;
            dataGridView3.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView3.Columns[1].HeaderText = "Monto Total";
            dataGridView3.Columns[1].ReadOnly = true;
            dataGridView3.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView3.Columns[2].HeaderText = "Monto Iva";
            dataGridView3.Columns[2].ReadOnly = true;
            dataGridView3.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView3.Columns[3].HeaderText = "Fecha";
            dataGridView3.Columns[3].ReadOnly = true;
            dataGridView3.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;           
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            loginForm = new LoginForm();
            loginForm.ShowDialog();

            lblNombre.Text = "Bienvenido " + nombre;

            if (!isEmploy) // modo cliente
            {
                lblPuntos.Text = "Actualmente tiene " + GetPuntos().ToString() + " puntos";
                tabModo.TabPages.Remove(tabCajero);
                tabModo.TabPages.Remove(tabAdmin);
                BindFacturas();
            }
            else if (!isAdmin) // modo cajero
            {
                tabModo.TabPages.Remove(tabCliente);
                tabModo.TabPages.Remove(tabAdmin);

                BindGrid();
                BindClientes();
            }
            else // modo admin
            {
                tabModo.TabPages.Remove(tabCliente);
                tabModo.TabPages.Remove(tabCajero);
                BindInventario();
            }                
        }

        /// <summary>
        /// Llena el datagridview con el listado de clientes
        /// </summary>
        public void BindClientes()
        {
            dataGridView2.DataSource = null;
            dataGridView2.Columns.Clear();

            MainForm.con.Open();
            using (SqlCommand cmd = new SqlCommand("SELECT * FROM VerClientes", con))
            {
                cmd.CommandType = CommandType.Text;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    dt2 = new DataTable();
                    sda.Fill(dt2);
                    dataGridView2.DataSource = dt2;
                }
            }
            MainForm.con.Close();

            dataGridView2.Columns[0].HeaderText = "CC";
            dataGridView2.Columns[0].ReadOnly = true;
            dataGridView2.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView2.Columns[1].HeaderText = "Nombre";
            dataGridView2.Columns[1].ReadOnly = true;
            dataGridView2.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView2.Columns[2].HeaderText = "Puntos acc";
            dataGridView2.Columns[2].ReadOnly = true;
            dataGridView2.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            DataGridViewCheckBoxColumn columna = new DataGridViewCheckBoxColumn();
            columna.AutoSizeMode= DataGridViewAutoSizeColumnMode.AllCells;
            columna.HeaderText = "";
            dataGridView2.Columns.Insert(3, columna);
        }

        public void BindInventario()
        {
            dataGridView4.DataSource = null;
            dataGridView4.Columns.Clear();

            MainForm.con.Open();
            using (SqlCommand cmd = new SqlCommand("SELECT * FROM VerInventario", con))
            {
                cmd.CommandType = CommandType.Text;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    dt = new DataTable();
                    sda.Fill(dt);
                    dataGridView4.DataSource = dt;
                }
            }
            MainForm.con.Close();

            dataGridView4.Columns[0].HeaderText = "Barcode";
            dataGridView4.Columns[0].ReadOnly = true;
            dataGridView4.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView4.Columns[1].HeaderText = "N Gondola";
            dataGridView4.Columns[1].ReadOnly = true;
            dataGridView4.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView4.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView4.Columns[2].HeaderText = "N Bodega";
            dataGridView4.Columns[2].ReadOnly = true;
            dataGridView4.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView4.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            dataGridView4.Columns.Add("N", "N");
            dataGridView4.Columns[3].CellTemplate.ValueType = typeof(int);
            dataGridView4.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView4.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            foreach (DataGridViewRow row in dataGridView4.Rows)
            {
                row.Cells[3].Value = 0;
            }
        }

        /// <summary>
        /// Llenar el Datagridview con el listado de productos y precios
        /// </summary>
        private void BindGrid()
        {
            dataGridView1.DataSource = null;
            dataGridView1.Columns.Clear();

            MainForm.con.Open();
            using (SqlCommand cmd = new SqlCommand("SELECT * FROM VerProductoPrecio", con))
            {
                cmd.CommandType = CommandType.Text;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    dt = new DataTable();
                    sda.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
            }
            MainForm.con.Close();

            // se elimina la primera columna que contiene el ID del producto
            dataGridView1.Columns.RemoveAt(0);

            dataGridView1.Columns[0].HeaderText = "Producto";
            dataGridView1.Columns[0].ReadOnly = true;
            dataGridView1.Columns[1].HeaderText = "Precio";
            dataGridView1.Columns[1].ReadOnly = true;
            dataGridView1.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            dataGridView1.Columns.Add("cantidad", "Cantidad");
            dataGridView1.Columns[2].CellTemplate.ValueType = typeof(int);
            dataGridView1.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                row.Cells[2].Value = 0;
            }
        }

        /// <summary>
        /// Método para obtener el número de puntos de un cliente
        /// </summary>
        /// <returns></returns>
        public int GetPuntos()
        {
            int puntos = 0;

            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();

                cmd = new SqlCommand("VerPuntos", MainForm.con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@user", cc));

                SqlDataReader rdr = cmd.ExecuteReader();

                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        puntos=rdr.GetInt32(0);
                    }
                }
                else
                {
                    MessageBox.Show("El usuario " + cc.ToString() + " no aparece listado en el sistema de puntos", "Sistema de puntos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                con.Close();
                return puntos;
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Ocurrió un error.\n\n" + ex.Message, "Error SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                con.Close();
                return -1;
            }            
        }

        private void btnProductos_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();

                cmd = new SqlCommand("SELECT * FROM VerProductos", MainForm.con);
                cmd.CommandType = CommandType.Text;
                
                SqlDataReader rdr = cmd.ExecuteReader();

                MessageBox.Show("Consulta realizada con exito", "Listado de productos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Ocurrió un error.\n\n" + ex.Message, "Error SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            con.Close();
        }

        private void dataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex == 2) 
            {
                int i;

                if (!int.TryParse(Convert.ToString(e.FormattedValue), out i))
                {
                    e.Cancel = true; // no es un número                    
                }
                else
                {
                    // the input is numeric 
                }
            }
        }

        private void btnComprar_Click(object sender, EventArgs e)
        {
            // 1) se crea una factura en la tabla facturacion
            // 2) se crean cada uno de los registros de los productos vendidos en la tabla ventas, asociados a la factura nueva
            // 3) se eliminan los productos comprados del anaquel
            // 4) se deberían lanzar los triggers de gondola y bodega

            // calcular un previo del precio total de la compra
            decimal precio = 0;
            decimal iprecio = 0;
            SqlCommand cmd;
            decimal iva = 0;
            decimal iiva = 0;
            int puntos = 0;
            int ipuntos = 0;
            List<int> ids = new List<int>();
            List<int> ns = new List<int>();
            
            MainForm.con.Open();

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (Convert.ToInt32(dataGridView1.Rows[i].Cells[2].Value) > 0)
                {
                    ids.Add(Convert.ToInt32(dt.Rows[i][0]));
                    ns.Add(Convert.ToInt32(dataGridView1.Rows[i].Cells[2].Value));

                    iprecio = Convert.ToDecimal(dataGridView1.Rows[i].Cells[1].Value) * Convert.ToDecimal(dataGridView1.Rows[i].Cells[2].Value);
                    precio += iprecio;

                    // obtener el iva y puntos

                    using (SqlCommand comando = new SqlCommand("GetIvaPuntos", MainForm.con))
                    {
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@id", Convert.ToInt32(dt.Rows[i][0])));
                        using(SqlDataReader rdr = comando.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                iiva = Convert.ToDecimal(rdr.GetInt32(0));
                                ipuntos = rdr.GetInt32(1);
                            }
                        }

                        iva += iprecio * iiva / 100;
                        puntos += ipuntos * Convert.ToInt32(dataGridView1.Rows[i].Cells[2].Value);
                    }                                        
                }
            }       

            if (MessageBox.Show("Desea realizar la compra por $" + Convert.ToString(precio) + "?","Verificación de compra",MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // obtener el total de facturas existentes
                
                DataTable dt3;
                using (SqlCommand cmd2 = new SqlCommand("SELECT * FROM GetCountFacturas", con))
                {
                    cmd2.CommandType = CommandType.Text;
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd2))
                    {
                        dt3 = new DataTable();
                        sda.Fill(dt3);                        
                    }
                }
                int nfactura = dt3.Rows.Count+1;

                int ifila = 0;
                using (SqlCommand cmd2 = new SqlCommand("CrearFactura", MainForm.con))
                {
                    cmd2.CommandType = CommandType.StoredProcedure;
                    cmd2.Parameters.Add(new SqlParameter("@id", nfactura));
                    cmd2.Parameters.Add(new SqlParameter("@monto", precio));
                    cmd2.Parameters.Add(new SqlParameter("@date", DateTime.Now));
                    cmd2.Parameters.Add(new SqlParameter("@montoIva", iva));

                    
                    for (int i = 0; i < dataGridView2.Rows.Count; i++)
                        if (Convert.ToBoolean(dataGridView2.Rows[i].Cells[3].Value) == true)
                        {
                            ifila = i;
                            break;
                        }
                    cmd2.Parameters.Add(new SqlParameter("@cliente", dt2.Rows[ifila][0]));
                    cmd2.Parameters.Add(new SqlParameter("@personal", MainForm.cc));
                    cmd2.ExecuteNonQuery();
                }
                
                for (int i = 0; i < ids.Count; i++)
                {
                    using (SqlCommand comando = new SqlCommand("ActualizarGondola", MainForm.con))
                    {
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@id", ids[i]));
                        comando.Parameters.Add(new SqlParameter("@n", ns[i]));
                        comando.ExecuteNonQuery();
                    }

                    using (SqlCommand comando = new SqlCommand("CrearVenta", MainForm.con))
                    {
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@id", ids[i]));
                        comando.Parameters.Add(new SqlParameter("@n", ns[i]));
                        comando.Parameters.Add(new SqlParameter("@factura", nfactura));
                        comando.ExecuteNonQuery();
                    }
                }

                cmd = new SqlCommand();
                cmd = new SqlCommand("ActualizarPuntos", MainForm.con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@id", dt2.Rows[ifila][0]));
                cmd.Parameters.Add(new SqlParameter("@n", puntos));
                cmd.ExecuteReader();

                MainForm.con.Close();

                MessageBox.Show("Monto total: $" + precio.ToString() + "\nIva: $" + iva.ToString() + "\nPuntos: " + puntos.ToString(),"Compra exitosa",MessageBoxButtons.OK,MessageBoxIcon.Information);
                BindClientes();
            }

            MainForm.con.Close();
            BindGrid();
        }

        

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddClienteForm agregar = new AddClienteForm();
            agregar.ShowDialog();
            BindClientes();
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int row_index = e.RowIndex;
            for (int i = 0; i < dataGridView2.Rows.Count; i++)
                if (row_index != i)
                    dataGridView2.Rows[i].Cells[3].Value = false;

            if (e.ColumnIndex == dataGridView2.Columns[3].Index)
                dataGridView2.EndEdit();                
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MainForm.con.Open();
            for (int i = 0; i < dataGridView4.Rows.Count; i++)
            {
                if (Convert.ToInt32(dataGridView4.Rows[i].Cells[3].Value) > 0)
                {
                    using (SqlCommand cmd = new SqlCommand("MoverAgondola", MainForm.con))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@codigo", dataGridView4.Rows[i].Cells[0].Value));
                            cmd.Parameters.Add(new SqlParameter("@n", dataGridView4.Rows[i].Cells[3].Value));
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
            MainForm.con.Close();
            BindInventario();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            BindInventario();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ComprarForm comprarForm = new ComprarForm();
            DataTable dt = new DataTable();

            MainForm.con.Open();
            for (int i = 0; i < dataGridView4.Rows.Count; i++)
            {
                if (Convert.ToInt32(dataGridView4.Rows[i].Cells[3].Value) > 0)
                {
                    using (SqlCommand cmd = new SqlCommand("GetInfoCompra", MainForm.con))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@codigo", dataGridView4.Rows[i].Cells[0].Value));
                            sda.Fill(dt);

                            comprarForm.dataGridView1.Rows.Add();
                            int n = comprarForm.dataGridView1.Rows.Count - 1;

                            comprarForm.dataGridView1.Rows[n].Cells[0].Value = dataGridView4.Rows[i].Cells[0].Value;
                            comprarForm.dataGridView1.Rows[n].Cells[1].Value = dataGridView4.Rows[i].Cells[3].Value;
                            comprarForm.dataGridView1.Rows[n].Cells[2].Value = dt.Rows[0][0];
                            comprarForm.dataGridView1.Rows[n].Cells[3].Value = dt.Rows[0][1];
                            comprarForm.dataGridView1.Rows[n].Cells[4].Value = dt.Rows[0][2];
                        }
                    }
                }
            }

            MainForm.con.Close();

            if (comprarForm.ShowDialog() == DialogResult.OK)
            {
                MainForm.con.Open();

                for (int i = 0; i < dataGridView4.Rows.Count; i++)
                {
                    if (Convert.ToInt32(dataGridView4.Rows[i].Cells[3].Value) > 0)
                    {
                        using (SqlCommand cmd = new SqlCommand("ComprarBodega", MainForm.con))
                        {
                            using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add(new SqlParameter("@codigo", dataGridView4.Rows[i].Cells[0].Value));
                                cmd.Parameters.Add(new SqlParameter("@n", dataGridView4.Rows[i].Cells[3].Value));
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                }

                MainForm.con.Close();
                BindInventario();
            }

           

            
        }
    }
}
