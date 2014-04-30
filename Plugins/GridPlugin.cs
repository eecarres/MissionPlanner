using MissionPlanner.Plugin;
using MissionPlanner.Utilities;
using SmartGridPlugin;
using System;
using System;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Linq;
using System.Linq;
using System.Text;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms;
using GMap.NET;
using GMap.NET.WindowsForms;

namespace SmartGridPlugin
{
    public class GridPlugin : MissionPlanner.Plugin.Plugin
    {
        public static MissionPlanner.Plugin.PluginHost Host2;

        /// <summary>
        /// El nombre del Plugin
        /// </summary>
        string _Name = "Plugin SmartGrid";
      
        /// <summary>
        /// La version del plugin
        /// </summary>
        string _Version = "0.1";
       
        /// <summary>
        /// El autor del Plugin
        /// </summary>
        string _Author = "Ethan Carrés";

        /// <summary>
        /// Devuelve el nombre del Plugin
        /// </summary>
        /// <value>
        /// Nombre del Plugin
        /// </value>
        public override string Name { get { return _Name; } }

        /// <summary>
        /// Devuelve la version del Plugin
        /// </summary>
        /// <value>
        /// Version
        /// </value>
        public override string Version { get { return _Version; } }
       
        /// <summary>
        /// Devuelve el autor
        /// </summary>
        /// <value>
        /// Autor del Plugin
        /// </value>
        public override string Author { get { return _Author; } }

        /// <summary>
        /// El botón que se verá en FP
        /// </summary>
        ToolStripMenuItem but = new ToolStripMenuItem("SmartGrid by Hemav");

         public List<GMapPolygon> listaPoligonos = new List<GMapPolygon>();

        public override bool Init()
        {
            return true;
        }

        /// <summary>
        /// La función que se llama al iniciar Mission Planner, que nos permite cargar el plugin 
        ///( introducir el botón correspondiente en el menú del Flight Planner y inicializar las variables necesarias)
        /// </summary>
        /// <returns>Cargado o no</returns>
        public override bool Loaded()
        {
            Host2 = Host;

            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GridUI));
            var temp = (string)(resources.GetObject("$this.Text"));

            

            bool hit = false;
            ToolStripItemCollection col = Host.FPMenuMap.Items;
            int index = col.Count;
            foreach (ToolStripItem item in col)
            {
                if (item.Text.Equals("Auto WP"))
                {
                    index = col.IndexOf(item);
                    ((ToolStripMenuItem)item).DropDownItems.Add(but);
                    hit = true;
                    break;
                }
            }

            if (hit == false)
                col.Add(but);

            but.Click += but_Click;

            return true;
        }

        /// <summary>
        /// Maneja la pulsacion del boton de inicio del plugin.
        /// </summary>
        /// <param name="sender">La fuente del evento</param>
        /// <param name="e"> e <see cref="EventArgs"/> Instancia con los datos del evento.</param>
        void but_Click(object sender, EventArgs e)
        {
            if (Host.FPDrawnPolygon != null && Host.FPDrawnPolygon.Points.Count > 2)
            {
                procesarPoligono();
            }
            else
            {
                CustomMessageBox.Show("Introduce primero un polígono");
            }
        }

        /// <summary>
        /// Con el polígono dibujado y los parámetros de configuración, divide el polígono y muestra la división. Si no es correcta, permite reconfigurar y volver a dividir. 
        /// Luego genera la ruta con el Grid
        /// </summary>
        void procesarPoligono()
        {
            bool divisionCorrecta = false;


            while (divisionCorrecta == false)
            {
                divisionCorrecta = ConfigurarDivision(divisionCorrecta); 
            }

            

            listaPoligonos.RemoveAt(0);
            //LLamar a los Grids!
            foreach (GMapPolygon poligono in listaPoligonos)
            {
               GridUI gridUI = new GridUI(this, poligono);
                gridUI.ShowDialog();

            }
            
        }

        private bool ConfigurarDivision(bool divisionCorrecta)
        {
            listaPoligonos.Clear();
            listaPoligonos.Add(Host.FPDrawnPolygon);
            double areaPoligono = Utilidades.calcpolygonarea(Host.FPDrawnPolygon.Points)/10000;
            SmartPluginConfigurador form = new SmartGridPlugin.SmartPluginConfigurador(areaPoligono);
            form.ShowDialog();

            List<PointLatLng> listaPuntosPoligonoInicial = new List<PointLatLng>();
            foreach (PointLatLng punto in Host.FPDrawnPolygon.Points)
            {
                listaPuntosPoligonoInicial.Add(punto);
            }

            switch (form.TipoDivision)
            {
                case 0: DivisionPoligono.divisionVertical(listaPuntosPoligonoInicial, listaPoligonos, decimal.ToDouble(form.AreaMaxima), decimal.ToDouble(form.DesplazamientoMaximo));
                    break;
                case 1: DivisionPoligono.divisionHorizontal(listaPuntosPoligonoInicial, listaPoligonos, decimal.ToDouble(form.AreaMaxima), decimal.ToDouble(form.DesplazamientoMaximo));
                    break;
                case 2: DivisionPoligono.divisionVertical(listaPuntosPoligonoInicial, listaPoligonos, decimal.ToDouble(form.AreaMaxima), decimal.ToDouble(form.DesplazamientoMaximo), form.Franjas);
                    break;
                case 3: DivisionPoligono.divisionHorizontal(listaPuntosPoligonoInicial, listaPoligonos, decimal.ToDouble(form.AreaMaxima), decimal.ToDouble(form.DesplazamientoMaximo), form.Franjas);
                    break;
            }


            // llamar a una ventana que nos muestre un mapa con los polígonos generados

            VistaPrevia vistaPrevia = new VistaPrevia(listaPoligonos);
            vistaPrevia.ShowDialog();

            divisionCorrecta = vistaPrevia.DivisionCorrecta;

            if (divisionCorrecta)
            {
                Host.config["grid_camera"] = form.Camara;
                Host.config["grid_alt"] = form.Altura.ToString();
                Host.config["grid_angle"] = "10";
                Host.config["grid_camdir"] = "True";

                Host.config["grid_dist"] = "10";
                Host.config["grid_overshoot1"] = form.OvershootHorizontal.ToString();
                Host.config["grid_overshoot2"] = form.OvershootVertical.ToString();
                Host.config["grid_overlap"] = form.Overlap.ToString();
                Host.config["grid_sidelap"] = form.Sidelap.ToString();
                Host.config["grid_spacing"] = "10";

                Host.config["grid_startfrom"] = "Home";

                Host.config["grid_advanced"] = "True";

            }

            return divisionCorrecta;
        }

        /// <summary>
        /// Exit is only called on plugin unload. not app exit
        /// </summary>
        /// <returns></returns>
        public override bool Exit()
        {
            return true;
        }
    }
}
