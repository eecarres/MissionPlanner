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
    public partial class ProgresoAngulo : Form
    {
        int anguloActual = 0;
        public ProgresoAngulo()
        {
            InitializeComponent();
            anguloActual = 0;
            lblProgreso.Text = anguloActual+"/180";
            //progressBar1.Step=100/180;
        }

        public void IncrementaUno()
        {
            progressBar1.PerformStep();
            progressBar1.Refresh();
            anguloActual++;
            lblProgreso.Text = anguloActual + "/180";
        }
    }
}
