using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


/// EDINSON ENRIQUE BORNACHERA BENITEZ
/// LUISANA DEL CARME GIL ROMERO
/// ELKER SNEIDER HERRA ZORRO
/// CALEB JAVIER DEVIA BERMUDEZ
namespace ORMPrac1
{
    public partial class Form1 : Form
    {
        // Se les agrega un nombre a el objetos
        public List<Model.ALUMNO> oAlumno;
        public List<Model.APODERADO> oApoderado;
        public List<Model.CURSO> oCurso;
        public List<Model.INSCRITO> oInscrito;
        int indice = 0;
        public Form1()
        {
            InitializeComponent();
        }

        //se agregan las tablas a el form1.cs "ORM PRACTICA 1"
        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Add("ALUMNO");
            comboBox1.Items.Add("APODERADO");
            comboBox1.Items.Add("CURSO");
            comboBox1.Items.Add("INSCRITO");
        }

        //Hacer que el comboBox1 pueda acceder a las tablas  de la base de datos
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (Model.DBPrac1Entity db = new Model.DBPrac1Entity())
            {
                switch(comboBox1.SelectedIndex)
                {
                    case 0:
                        oAlumno = db.ALUMNO.ToList();
                        break;
                    case 1:
                        oApoderado = db.APODERADO.ToList();
                        break;
                    case 2: oCurso = db.CURSO.ToList(); 
                        break;
                    case 3: oInscrito = db.INSCRITO.ToList();
                        break;
                }
            }
            indice = 0;
            //metodo llenar que es quien valida/ busca las relaciones
            Llenar();
        }

        //hace que el button1 (boton 1 Anterior) al hacer click retroceda a el dato anterior
        private void button1_Click(object sender, EventArgs e)
        {
            indice--;
            Llenar();
        }

        //hace que el button2 (boton 2 Siguiente) al hacer click avanze a el dato siguiente
        private void button2_Click(object sender, EventArgs e)
        {
            indice++;
            Llenar();
        }
        //Programacion del metodo llenar
        public void Llenar()
        {
            if (indice < 0)
                indice = 0;

            string cadena = "";
            switch (comboBox1.SelectedIndex)
            {
                // En el caso 0 hace una busqueda en la tabla de alumnos
                case 0:
                    if (indice >= oAlumno.Count)
                        indice = oAlumno.Count - 1;

                    cadena = oAlumno[indice].Id.ToString() + "."+oAlumno[indice].Nombre + ", de"+oAlumno[indice].Ciudad+", " +oAlumno[indice].Edad  + " Años";
                    break;
                // En el caso se usa Find y DBPrac1Entity para buscar en las distintas tablas 
                case 1:
                    if (indice >= oApoderado.Count)
                        indice = oApoderado.Count - 1;

                    using (Model.DBPrac1Entity db = new Model.DBPrac1Entity())
                    {
                        oAlumno = db.ALUMNO.ToList();
                // lo primero es traer el id de apoderado por medio del objeto que seria el nombre, luego se evalua los id de alumno y apoderado para ver si hay alguna relacion y al final simplemente se trae el nombre el alumno
                        cadena = oApoderado[indice].Id.ToString() + ". " + oApoderado[indice].Nombre + ", es el | la apoderado|a de " + oAlumno.Find(a => a.Id == (int)oApoderado[indice].Id_alumno).Nombre;
                    }
                    break;

                // En el caso 2 hace una busqueda en la tabla de alumnos
                case 2:
                    if (indice >= oCurso.Count)
                        indice = oCurso.Count - 1;

                    cadena = oCurso[indice].Cod.ToString() + ". " + oCurso[indice].Nombre + ", Fecha Inicio " + oCurso[indice].Fecha_Inicio + ", Tiempo de Duracion" + oCurso[indice].Duracion + ", Valor " + oCurso[indice].Valor;  
                    break;
                // En el caso 3 hace una busqueda en la tabla de inscrito alumnos y tambien de alumno y se usa el Find nuevamente para validar datos iguales en las tablas
                case 3:
                    if (indice >= oInscrito.Count)
                        indice = oInscrito.Count - 1;
                    using (Model.DBPrac1Entity db = new Model.DBPrac1Entity())
                    {
                        oAlumno = db.ALUMNO.ToList();
                        oCurso = db.CURSO.ToList();
                        
                        
                        cadena = oInscrito[indice].Id.ToString() + ". " + oAlumno.Find(a => a.Id == (int)oInscrito[indice].Id_Alumno).Nombre + "  Está Estudiando        " + oCurso.Find(a => a.Cod == (int)oInscrito[indice].Cod_Curso).Nombre; 
                    
                    }
                    break;

                  
            }
            //luego en el textBox1 mostramos cadena que es la ejecucion segun el caso del switch
            textBox1.Text = cadena;
        }
    }

}
