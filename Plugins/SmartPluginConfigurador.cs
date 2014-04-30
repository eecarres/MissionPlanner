using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SmartGridPlugin
{
    public partial class SmartPluginConfigurador : Form
    {

        // Propiedades de la pestaña GRID
        private string plataforma = "";
        private string camara = "";
        private int altura = 0;
        private decimal overshootHorizontal = 0;
        public decimal OvershootHorizontal
        {
            get { return overshootHorizontal; }
        }
        private decimal overshootVertical = 0;
        public decimal OvershootVertical
        {
            get { return overshootVertical; }
        }
        private decimal overlap = 50;
        public decimal Overlap
        {
            get { return overlap; }
        }
        private decimal sidelap = 60;
        public decimal Sidelap
        {
            get { return sidelap; }
        }

        public int Altura
        {
            get { return altura; }
        }
        public string Camara
        {
            get { return camara; }
        }
        public string Plataforma
        {
            get { return this.plataforma; }
        }
        
        // Propiedades de la pestaña Division de  areas

        private decimal areaMaxima = 30;
        public decimal AreaMaxima
        {
            get { return areaMaxima; }
        }
        private decimal desplazamientoMaximo = 0.5m;
        public decimal DesplazamientoMaximo
        {
            get { return desplazamientoMaximo; }
        }
        private int tipoDivision = 0;
        public int TipoDivision
        {
            get { return tipoDivision; }
        }
        private int franjas = 2;
        public int Franjas
        {
            get { return franjas; }
        }


          // Métodos del plugin  
        public SmartPluginConfigurador(double areaPoligono)
        {
            InitializeComponent();
            lblFranjas.Hide();
            numFranjas.Hide();
            lblArea.Text = areaPoligono.ToString();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cmbPlataforma_SelectedIndexChanged(object sender, EventArgs e)
        {
            plataforma=cmbPlataforma.SelectedItem.ToString();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            camara = cmbCamara.SelectedItem.ToString();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            altura = Int32.Parse(numAltura.Value.ToString());
        }

        private void numOvershootHorizontal_ValueChanged(object sender, EventArgs e)
        {
            overshootHorizontal = numOvershootHorizontal.Value;
        }

        private void numOvershootVertical_ValueChanged(object sender, EventArgs e)
        {
            overshootVertical = numOvershootVertical.Value;
        }

        private void numOverlap_ValueChanged(object sender, EventArgs e)
        {
            overlap = numOverlap.Value;
        }

        private void numSidelap_ValueChanged(object sender, EventArgs e)
        {
            sidelap = numSidelap.Value;
        }

        private void numAreaMaxima_ValueChanged(object sender, EventArgs e)
        {
            areaMaxima = numAreaMaxima.Value;
        }

        private void numDespMaximo_ValueChanged(object sender, EventArgs e)
        {
            desplazamientoMaximo = numDespMaximo.Value;
        }

        private void cmbTipoDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            tipoDivision = cmbTipoDivision.SelectedIndex;
            if (tipoDivision==2|| tipoDivision==3)
            {
                lblFranjas.Show();
                numFranjas.Show();
            }
            else
            {
                lblFranjas.Hide();
                numFranjas.Hide();
            }
        }

        private void numFranjas_ValueChanged(object sender, EventArgs e)
        {
            franjas = decimal.ToInt16(numFranjas.Value);
        }

        private void SmartPluginConfigurador_Load(object sender, EventArgs e)
        {
            franjas = decimal.ToInt16(numFranjas.Value);
            
        }
    }
}
