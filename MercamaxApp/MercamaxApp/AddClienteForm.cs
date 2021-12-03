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
    public partial class AddClienteForm : Form
    {
        public AddClienteForm()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtCc.Text))
                return;
            if (String.IsNullOrEmpty(txtDireccion.Text))
                return;
            if (String.IsNullOrEmpty(txtEmail.Text))
                return;
            if (String.IsNullOrEmpty(txtNombre.Text))
                return;
            if (String.IsNullOrEmpty(txtTelefono.Text))
                return;

            MainForm.con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("CrearCliente", MainForm.con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@cc", Convert.ToInt32(txtCc.Text)));
                cmd.Parameters.Add(new SqlParameter("@nombre", txtNombre.Text));
                cmd.Parameters.Add(new SqlParameter("@telefono", txtTelefono.Text));
                cmd.Parameters.Add(new SqlParameter("@email", txtEmail.Text));
                cmd.Parameters.Add(new SqlParameter("@direccion", txtDireccion.Text));
                cmd.Parameters.Add(new SqlParameter("@fecha", dateTimePicker1.Value));

                SqlDataReader rdr = cmd.ExecuteReader();
                MainForm.con.Close();
                this.Close();
            }
            catch(SqlException exe)
            {
                MainForm.con.Close();
                MessageBox.Show("No fue posible crear el nuevo cliente. Por favor revise la información ingresada", "Error al crear el cliente", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            
        }
    }
}
