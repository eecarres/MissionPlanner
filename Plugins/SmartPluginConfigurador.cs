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
    /// <summary>
    /// El formulario de configuración del PlugIn
    /// </summary>
    public partial class SmartPluginConfigurador : Form
    {
        GridPlugin plugin;

        // Propiedades de la pestaña GRID
        private bool anguloOptimo = true;
        public bool AnguloOptimo
        {
            get { return anguloOptimo; }
        }
        private bool divisionRecta = false;
        public bool DivisionRecta
        {
            get { return divisionRecta; }
        }
        private string nombreMision = "";
        public string NombreMision
        {
            get { return nombreMision; }
        }
        bool mostrarGridUI = false;
        public bool MostrarGridUI
        {
            get { return mostrarGridUI; }
        }
        private string plataforma = "";
        private string camara = "";
        private decimal altura = 150;
        private decimal angulo = 0;

        public decimal Angulo
        {
            get { return angulo; }
        }

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

        public decimal Altura
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

        private decimal areaMaxima = 35;
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
        public SmartPluginConfigurador(double areaPoligono,GridPlugin gridPlugin)
        {
            InitializeComponent();
            lblFranjas.Hide();
            numFranjas.Hide();
            string textoArea = String.Format("{0:0.00}", areaPoligono);
            lblArea.Text = textoArea + " Ha";
            cmbCamara.SelectedIndex = 0;
            cmbPlataforma.SelectedIndex = 0;
            numAngulo.Enabled = false;
            plugin = gridPlugin;
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

        private void chckMostrarGridUI_CheckedChanged(object sender, EventArgs e)
        {
            mostrarGridUI = chckMostrarGridUI.Checked;
        }

        private void numAngulo_ValueChanged(object sender, EventArgs e)
        {
            angulo = numAngulo.Value;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            nombreMision = txtNombreMision.Text;
        }

        private void chckRecta_CheckedChanged(object sender, EventArgs e)
        {
            divisionRecta = chckRecta.Checked;
        }

        private void chkAnguloOptimo_CheckedChanged(object sender, EventArgs e)
        {
            anguloOptimo = chkAnguloOptimo.Checked;
            if (anguloOptimo)
            {
                numAngulo.Enabled = false;
            }
            else
            {
                numAngulo.Enabled = true;
            }

        }


        void loadsettings()
        {
            if (plugin.Host.config.ContainsKey("grid_camera"))
            {
                loadsetting("grid_alt",numAltura );
                loadsetting("grid_angle", numAngulo);
                loadsetting("grid_overshoot1", numOvershootHorizontal);
                loadsetting("grid_overshoot2", numOvershootVertical);
                loadsetting("grid_overlap", numOverlap);
                loadsetting("grid_sidelap", numSidelap);
                // camera last to it invokes a reload
                loadsetting("grid_camera", cmbCamara);

            }
        }

        void loadsetting(string key, Control item)
        {
            if (plugin.Host.config.ContainsKey(key))
            {
                if (item is NumericUpDown)
                {
                    ((NumericUpDown)item).Value = decimal.Parse(plugin.Host.config[key].ToString());
                }
                else if (item is ComboBox)
                {
                    ((ComboBox)item).Text = plugin.Host.config[key].ToString();
                }
                else if (item is CheckBox)
                {
                    ((CheckBox)item).Checked = bool.Parse(plugin.Host.config[key].ToString());
                }
                else if (item is RadioButton)
                {
                    ((RadioButton)item).Checked = bool.Parse(plugin.Host.config[key].ToString());
                }
            }
        }







    }
}
