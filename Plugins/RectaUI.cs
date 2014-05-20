using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
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
    public partial class RectaUI : Form
    {
        public List<List<PointLatLng>> listaPuntosPoligonos = new List<List<PointLatLng>>();
        public List<GMapPolygon> listaPoligono=new List<GMapPolygon>();
        PointLatLng puntoRecta1 = new PointLatLng(0, 0);
        PointLatLng puntoRecta2 = new PointLatLng(0, 0);
       
        GMapPolygon poligono;
        GMapOverlay overlayRecta = new GMapOverlay();
        GMapOverlay overlayRuta = new GMapOverlay();
        public RectaUI(GMapPolygon poligonoACargar)
        {
            InitializeComponent();
            poligono = poligonoACargar;
        }

        private void map_Load(object sender, EventArgs e)
        {
            map.MapProvider = GMap.NET.MapProviders.BingSatelliteMapProvider.Instance; // Seleccionamos el proveedor de mapas
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerAndCache; // Seleccionamos si guardamos caché o solo online
            map.Position =poligono.Points[0];  // Seleccionamos la posición que queremos que apunte en el inicio
            
            map.MaxZoom = 25;
            GMapOverlay overlayPoligonos = new GMapOverlay();
            overlayPoligonos.Polygons.Add(poligono);
            map.Overlays.Add(overlayPoligonos);
            map.Refresh();
            map.Zoom = 16;
            map.DragButton = MouseButtons.Right;
           
        }

        private void map_Click(object sender, EventArgs e)
        {
           
               Point puntoPantalla = new Point(Cursor.Position.X, Cursor.Position.Y); // puntoPantalla es el punto en absolutas
                puntoPantalla = map.PointToClient(puntoPantalla);
                if (puntoRecta1.Lat == 0.0)
                {
                    puntoRecta1 = map.FromLocalToLatLng(puntoPantalla.X, puntoPantalla.Y);
                    GMarkerGoogle markerRecta1 = new GMarkerGoogle(puntoRecta1, GMarkerGoogleType.red);
                    overlayRecta.Markers.Add(markerRecta1);
                    map.UpdateMarkerLocalPosition(markerRecta1);
                    map.Overlays.Add(overlayRecta);
                }
                else
                {
                    puntoRecta2 = map.FromLocalToLatLng(puntoPantalla.X, puntoPantalla.Y);

                    GMarkerGoogle markerRecta2 = new GMarkerGoogle(puntoRecta2, GMarkerGoogleType.green);
                    overlayRecta.Markers.Add(markerRecta2);
                    map.Overlays.Add(overlayRecta); 
                    map.UpdateMarkerLocalPosition(markerRecta2);
                    List<PointLatLng> puntosRuta = new List<PointLatLng>();
                    puntosRuta.Add(puntoRecta1);
                    puntosRuta.Add(puntoRecta2);
                    GMapRoute Ruta = new GMapRoute(puntosRuta, "ruta");
                    overlayRuta.Routes.Add(Ruta);
                    map.Overlays.Add(overlayRuta);
                    map.Refresh();
                     Recta rectaDivisoria = new Recta(puntoRecta1, puntoRecta2);

                    MessageBox.Show("Recta definida");
                    //listaPuntosPoligonos[0].Clear();
                    //listaPuntosPoligonos[1].Clear();
                    listaPuntosPoligonos=DivisorRecta.Separacion(poligono.Points, rectaDivisoria, listaPoligono);
                    this.Close();
                }

        }
    }
}
