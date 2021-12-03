using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MercamaxApp
{
    public partial class ComprarForm : Form
    {
        public ComprarForm()
        {
            InitializeComponent();
        }

        private void ComprarForm_Load(object sender, EventArgs e)
        {
            
        }

        private void btnComprar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
