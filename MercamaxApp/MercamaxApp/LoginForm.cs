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
using System.IO;

namespace MercamaxApp
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Está seguro que desea salir?","Confirmación de salida",MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        /// <summary>
        /// Error de autenticación
        /// </summary>
        public void ErrorLogin()
        {
            if (MessageBox.Show("Usuario/contraseña no reconocidos.\n\nDesea intentarlo de nuevo?", "Error de auttenticación", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.No)
            {
                Application.Exit();
            }
            else
            {
                txtPass.Clear();
                txtUser.Clear();
            }
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            try
            {
                MainForm.con.Open();

                SqlCommand cmd = new SqlCommand(); ;

                switch (radClient.Checked)
                {
                    case true:
                        cmd = new SqlCommand("LoginCliente", MainForm.con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@user", Convert.ToInt32(txtUser.Text)));
                        break;
                    case false:
                        cmd = new SqlCommand("Login", MainForm.con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@user", Convert.ToInt32(txtUser.Text)));
                        cmd.Parameters.Add(new SqlParameter("@pass", txtPass.Text));
                        break;
                }
                
                SqlDataReader rdr = cmd.ExecuteReader();                

                if (!rdr.HasRows)
                {
                    ErrorLogin();
                }
                else
                {
                    MainForm.cc = Convert.ToInt32(txtUser.Text);
                    switch (radClient.Checked)
                    {
                        case true:
                            MainForm.isEmploy = false;
                            MainForm.isAdmin = false;
                            while (rdr.Read())
                            {
                                MainForm.nombre = rdr.GetString(1);
                            }                            
                            break;
                        case false:
                            MainForm.isEmploy = true;
                            while (rdr.Read())
                            {
                                MainForm.isAdmin = rdr.GetBoolean(1);
                                MainForm.nombre = rdr.GetString(2);
                            }
                            break;
                    }

                    this.Close();
                }

                MainForm.con.Close();
            }
            catch
            {
                MainForm.con.Close();

                ErrorLogin();
            }
        }

        private void btnPreparar_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Este método reseteará y poblará la base de datos con una colección de registros\n\nEstá seguro que desea continuar?", "Advertencia", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                #region borrar todas las tablas

                MainForm.con.Open();
                string query = null;
                SqlCommand myCommand = null;

                query = @"DELETE FROM Producto";
                myCommand = new SqlCommand(query, MainForm.con);
                myCommand.ExecuteNonQuery();

                query = @"DELETE FROM Tipo_Producto";
                myCommand = new SqlCommand(query, MainForm.con);
                myCommand.ExecuteNonQuery();

                query = @"DELETE FROM Proveedor";
                myCommand = new SqlCommand(query, MainForm.con);
                myCommand.ExecuteNonQuery();

                query = @"DELETE FROM LugarStock";
                myCommand = new SqlCommand(query, MainForm.con);
                myCommand.ExecuteNonQuery();

                query = @"DELETE FROM Facturacion";
                myCommand = new SqlCommand(query, MainForm.con);
                myCommand.ExecuteNonQuery();

                query = @"DELETE FROM Cliente";
                myCommand = new SqlCommand(query, MainForm.con);
                myCommand.ExecuteNonQuery();

                query = @"DELETE FROM Personal";
                myCommand = new SqlCommand(query, MainForm.con);
                myCommand.ExecuteNonQuery();

                query = @"DELETE FROM Venta";
                myCommand = new SqlCommand(query, MainForm.con);
                myCommand.ExecuteNonQuery();

                

                MainForm.con.Close();   

                #endregion

                #region leer archivo de tipo de productos

                string path = "Tipo_producto.csv";
                string[] lines = System.IO.File.ReadAllLines(path);

                MainForm.con.Open();
                for (int i = 1; i < lines.Length; i++)
                {
                    string[] columns = lines[i].Split(';');

                    query = @"INSERT INTO Tipo_Producto (id_tipo,categoria,perecedero,iva,puntos) Values(@id_tipo,@categoria,@perecedero,@iva,@puntos)";
                    using (var com = new SqlCommand(query, MainForm.con))
                    {
                        // com.Parameters.AddWithValue("@ID", ID);
                        com.Parameters.AddWithValue("@id_tipo", Convert.ToInt32(columns[0]));
                        com.Parameters.AddWithValue("@categoria", columns[1]);
                        com.Parameters.AddWithValue("@perecedero", columns[2]=="1");
                        com.Parameters.AddWithValue("@iva", Convert.ToInt32(columns[3]));
                        com.Parameters.AddWithValue("@puntos", Convert.ToInt32(columns[4]));
                        try
                        {
                            com.ExecuteNonQuery();
                        }
                        catch (SqlException exe)
                        {
                            MessageBox.Show(exe.Message);
                        }
                    }
                }
                MainForm.con.Close();

                #endregion

                #region leer archivo de tipo de proveedores

                path = "Proveedor.csv";
                lines = System.IO.File.ReadAllLines(path);

                MainForm.con.Open();
                
                for (int i = 1; i < lines.Length; i++)
                {
                    string[] columns = lines[i].Split(';');

                    query = @"INSERT INTO Proveedor (nit,nombre_proveedor,telefono_proveedor,email_proveedor,direccion_proveedor,agente_ventas) Values(@nit,@nombre_proveedor,@telefono_proveedor,@email_proveedor,@direccion_proveedor,@agente_ventas)";
                    using (var com = new SqlCommand(query, MainForm.con))
                    {
                        // com.Parameters.AddWithValue("@ID", ID);
                        com.Parameters.AddWithValue("@nit", Convert.ToInt32(columns[0]));
                        com.Parameters.AddWithValue("@nombre_proveedor", columns[1]);
                        com.Parameters.AddWithValue("@telefono_proveedor", columns[2]);
                        com.Parameters.AddWithValue("@email_proveedor", columns[3]);
                        com.Parameters.AddWithValue("@direccion_proveedor", columns[4]);
                        com.Parameters.AddWithValue("@agente_ventas", columns[5]);
                        try
                        {
                            com.ExecuteNonQuery();
                        }
                        catch (SqlException exe)
                        {
                            MessageBox.Show(exe.Message);
                        }
                    }
                }
                MainForm.con.Close();

                #endregion

                #region leer archivo de tipo de productos

                path = "Producto.csv";
                lines = System.IO.File.ReadAllLines(path);

                MainForm.con.Open();
                
                for (int i = 1; i < lines.Length; i++)
                {
                    string[] columns = lines[i].Split(';');

                    query = @"INSERT INTO Producto (id_producto,nombre_producto,fecha_llegada,fecha_vencimiento,barcode,precio,nit,id_tipo) Values(@id_producto,@nombre_producto,@fecha_llegada,@fecha_vencimiento,@barcode,@precio,@nit,@id_tipo)";
                    using (var com = new SqlCommand(query, MainForm.con))
                    {
                        // com.Parameters.AddWithValue("@ID", ID);
                        com.Parameters.AddWithValue("@id_producto", Convert.ToInt32(columns[0]));
                        com.Parameters.AddWithValue("@nombre_producto", columns[1]);
                        string[] columna = columns[2].Split('/');
                        com.Parameters.AddWithValue("@fecha_llegada", new DateTime(Convert.ToInt32(columna[2]), Convert.ToInt32(columna[1]), Convert.ToInt32(columna[0])));
                        columna = columns[3].Split('/');
                        com.Parameters.AddWithValue("@fecha_vencimiento", new DateTime(Convert.ToInt32(columna[2]), Convert.ToInt32(columna[1]), Convert.ToInt32(columna[0])));
                        com.Parameters.AddWithValue("@barcode", Convert.ToInt32(columns[4]));
                        com.Parameters.AddWithValue("@precio", Convert.ToInt32(columns[5]));
                        com.Parameters.AddWithValue("@nit", Convert.ToInt32(columns[6]));
                        com.Parameters.AddWithValue("@id_tipo", Convert.ToInt32(columns[7]));
                        try
                        {
                            com.ExecuteNonQuery();
                        }
                        catch (SqlException exe)
                        {
                            MessageBox.Show(exe.Message);
                        }
                    }
                }
                MainForm.con.Close();

                #endregion

                #region leer archivo de tipo de clientes

                path = "Cliente.csv";
                lines = System.IO.File.ReadAllLines(path);

                MainForm.con.Open();

                for (int i = 1; i < lines.Length; i++)
                {
                    string[] columns = lines[i].Split(';');

                    query = @"INSERT INTO Cliente (cc_cliente,nombre_apellido_cliente,telefono_cliente,email_cliente,direccion_cliente,fecha_nacimiento,puntos_acumulados) Values(@cc_cliente,@nombre_apellido_cliente,@telefono_cliente,@email_cliente,@direccion_cliente,@fecha_nacimiento,@puntos_acumulados)";
                    using (var com = new SqlCommand(query, MainForm.con))
                    {
                        // com.Parameters.AddWithValue("@ID", ID);
                        com.Parameters.AddWithValue("@cc_cliente", Convert.ToInt32(columns[0]));
                        com.Parameters.AddWithValue("@nombre_apellido_cliente", columns[1]);
                        com.Parameters.AddWithValue("@telefono_cliente", columns[2]);
                        com.Parameters.AddWithValue("@email_cliente", columns[3]);
                        com.Parameters.AddWithValue("@direccion_cliente", columns[4]);
                        string[] columna = columns[5].Split('/');
                        com.Parameters.AddWithValue("@fecha_nacimiento", new DateTime(Convert.ToInt32(columna[2]), Convert.ToInt32(columna[1]), Convert.ToInt32(columna[0])));
                        com.Parameters.AddWithValue("@puntos_acumulados", Convert.ToInt32(columns[6]));
                        try
                        {
                            com.ExecuteNonQuery();
                        }
                        catch (SqlException exe)
                        {
                            MessageBox.Show(exe.Message);
                        }
                    }
                }
                MainForm.con.Close();

                #endregion

                #region leer archivo de tipo de personal

                path = "Personal.csv";
                lines = System.IO.File.ReadAllLines(path);

                MainForm.con.Open();

                for (int i = 1; i < lines.Length; i++)
                {
                    string[] columns = lines[i].Split(';');

                    query = @"INSERT INTO Personal (cc_personal,nombre_apellido_personal,cargo,telefono_personal,email_personal,direccion_personal,fecha_nacimiento_personal,password) Values(@cc_personal,@nombre_apellido_personal,@cargo,@telefono_personal,@email_personal,@direccion_personal,@fecha_nacimiento_personal,@password)";
                    using (var com = new SqlCommand(query, MainForm.con))
                    {
                        // com.Parameters.AddWithValue("@ID", ID);
                        com.Parameters.AddWithValue("@cc_personal", Convert.ToInt32(columns[0]));
                        com.Parameters.AddWithValue("@nombre_apellido_personal", columns[1]);
                        com.Parameters.AddWithValue("@cargo", columns[2]=="1");
                        com.Parameters.AddWithValue("@telefono_personal", columns[3]);
                        com.Parameters.AddWithValue("@email_personal", columns[4]);
                        com.Parameters.AddWithValue("@direccion_personal", columns[5]);
                        string[] columna = columns[6].Split('/');
                        com.Parameters.AddWithValue("@fecha_nacimiento_personal", new DateTime(Convert.ToInt32(columna[2]), Convert.ToInt32(columna[1]), Convert.ToInt32(columna[0])));
                        com.Parameters.AddWithValue("@password", columns[7]);
                        try
                        {
                            com.ExecuteNonQuery();
                        }
                        catch (SqlException exe)
                        {
                            MessageBox.Show(exe.Message);
                        }
                    }
                }
                MainForm.con.Close();

                #endregion

                #region leer archivo de tipo de LugarStock

                path = "LugarStock.csv";
                lines = System.IO.File.ReadAllLines(path);

                MainForm.con.Open();

                for (int i = 1; i < lines.Length; i++)
                {
                    string[] columns = lines[i].Split(';');

                    query = @"INSERT INTO LugarStock (barcode_producto,cantidad_bodega,cantidad_gondola,seccion_bodega,seccion_gondola) Values(@barcode_producto,@cantidad_bodega,@cantidad_gondola,@seccion_bodega,@seccion_gondola)";
                    using (var com = new SqlCommand(query, MainForm.con))
                    {
                        // com.Parameters.AddWithValue("@ID", ID);
                        com.Parameters.AddWithValue("@barcode_producto", Convert.ToInt32(columns[0]));
                        com.Parameters.AddWithValue("@cantidad_bodega", Convert.ToInt32(columns[1]));
                        com.Parameters.AddWithValue("@cantidad_gondola", Convert.ToInt32(columns[2]));
                        com.Parameters.AddWithValue("@seccion_bodega", columns[3]);
                        com.Parameters.AddWithValue("@seccion_gondola", columns[4]);               
                        try
                        {
                            com.ExecuteNonQuery();
                        }
                        catch (SqlException exe)
                        {
                            MessageBox.Show(exe.Message);
                        }
                    }
                }
                MainForm.con.Close();

                #endregion

                MessageBox.Show("Base de datos poblada exitosamente", "Operación completada con exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        
    }
}
