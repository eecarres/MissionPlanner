using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GMap.NET.WindowsForms;
using GMap.NET;


namespace SmartGridPlugin
{
    /// <summary>
    /// Muestra la división tal cual está realizada con la configuración escogida
    /// </summary>
    public partial class VistaPrevia : Form
    {
        bool divisionCorrecta = false;

        public bool DivisionCorrecta
        {
            get { return divisionCorrecta; }
        }

       List<GMapPolygon> PoligonosAMostrar = new List<GMapPolygon>();
        public VistaPrevia(List<GMapPolygon> ListaPoligonos)
        {
            InitializeComponent();
            foreach (GMapPolygon poligono in ListaPoligonos)
            {
                PoligonosAMostrar.Add(poligono);
            }
        }

        private void VistaPrevia_Load(object sender, EventArgs e)
        {

        }

        private void map_Load(object sender, EventArgs e)
        {
            map.MapProvider = GMap.NET.MapProviders.BingSatelliteMapProvider.Instance; // Seleccionamos el proveedor de mapas
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerAndCache; // Seleccionamos si guardamos caché o solo online
            map.Zoom = 16;
            map.MaxZoom = 25;
            map.Position = PoligonosAMostrar[0].Points[0]; // Seleccionamos la posición que queremos que apunte en el inicio

            GMapOverlay overlayPoligonos = new GMapOverlay();

            foreach (GMapPolygon poligono in PoligonosAMostrar)

            {
                overlayPoligonos.Polygons.Add(poligono);
                
            }

            map.Overlays.Add(overlayPoligonos);
            map.Refresh();

            map.DragButton = MouseButtons.Left;
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            divisionCorrecta = true;
            this.Close();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
