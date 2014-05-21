using MissionPlanner.Plugin;
using MissionPlanner.Utilities;
using SmartGridPlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GMap.NET;
using GMap.NET.WindowsForms;

/// <summary>
/// El namespace del PlugIn
/// </summary>
namespace SmartGridPlugin
{
    /// <summary>
    ///  La clase GridPlugin es una instancia de la clase estática Plugin de Mission Planner
    /// </summary>
    public class GridPlugin : MissionPlanner.Plugin.Plugin
    {
        /// <summary>
        /// Instancia del Host, objeto que nos permite acceder a los datos de la aplicación principal (menús, polígonos dibujados, etc...)
        /// </summary>
        public static MissionPlanner.Plugin.PluginHost Host2;

        /// <summary>
        /// El nombre del Plugin
        /// </summary>
        string _Name = "Plugin SmartGrid";
      
        /// <summary>
        /// La version del plugin
        /// </summary>
        string _Version = "0.2";
       
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

        /// <summary>
        /// La lista de los subpolígonos que se crearán a partir del polígono original
        /// </summary>
         public List<GMapPolygon> listaPoligonos = new List<GMapPolygon>();

         /// <summary>
         /// Maneja la opción de mostrar o no mostrar cada una de las submisiones para configurarlas independientemente.
         /// </summary>
         public bool mostrarGridUI = false;

         /// <summary>
         /// Determina el nombre de la carpeta y las misiones
         /// </summary>
         public string nombreMision = "";

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
            // 
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
            // Variable que determina si la división tiene la aprobación del usuario
            bool divisionCorrecta = false;
            

            // Llamamos al configurador, que pide al usario unos parámetros y luego propone una solución. Si es correcta, continua el programa
            while (divisionCorrecta == false)
            {
                divisionCorrecta = ConfigurarDivision();
            }

            //listaPoligonos.RemoveAt(0);
            // Llama  a los grids
            for (int i = 0; i< listaPoligonos.Count() ; i++)
			{

                // En primer lugar escribimos un takeoff que varía en función de la plataforma
                if (this.Host.cs.firmware == MissionPlanner.MainV2.Firmwares.ArduCopter2)
                {
                    this.Host.AddWPtoList(MAVLink.MAV_CMD.TAKEOFF, 0, 0, 0, 0, 0, 0, 30);
                }
                else
                {
                    this.Host.AddWPtoList(MAVLink.MAV_CMD.TAKEOFF, 20, 0, 0, 0, 0, 0, 30);
                }


                // Inicializamos un objeto GridUI
               GridUI gridUI = new GridUI(this, listaPoligonos[i]);
                // tan sólo se muestra la interfaz visual si así se ha requerido en la configuración
                if (mostrarGridUI==true)
                {gridUI.ShowDialog();}
                // en caso contrario se acepta la misión con los parámetros especificados en el configurador
                else
                {gridUI.aceptarSinPrevisualizar();}

                //Añadimos puntos de LANDING

                if (this.Host.cs.firmware == MissionPlanner.MainV2.Firmwares.ArduCopter2)
                {
                    this.Host.AddWPtoList(MAVLink.MAV_CMD.LAND, 0, 0, 0, 0, 0, 0, 30);
                }
                else
                {
                    this.Host.AddWPtoList(MAVLink.MAV_CMD.LAND, 20, 0, 0, 0, 0, 0, 30);
                }
               
                // Guardar cada uno de los poligonos como misión independiente
                MissionPlanner.MainV2.instance.FlightPlanner.guardarMision(nombreMision,i);


                // Actualizar KML
                MissionPlanner.MainV2.instance.FlightPlanner.writeKML();

                // Borramos los puntos de las misiones anteriores 
                MissionPlanner.MainV2.instance.FlightPlanner.borrarMision();

            }
            
        }

        /// <summary>
        /// Muestra un formulario que permite configurar el tipo de plataforma, payload y tipo de división de áreas
        /// </summary>
        /// <returns></returns>
        private bool ConfigurarDivision()
        {
            // Determina si debe volver a configurarse la división
            bool divisionCorrecta;
            bool divisionRecta;
            // Limpiamos la lista de polígonos (por si no es la primera configuración)
            listaPoligonos.Clear();
            // El primer polígono es siempre el original, dibujado por el usuario en la pestaña Mission PLanner
            List<List<PointLatLng>> listaPuntosPoligonos = new List<List<PointLatLng>>();
            // Se obtiene el área del polígono
            double areaPoligono = Utilidades.calcpolygonarea(Host.FPDrawnPolygon.Points)/10000;
            //Se llama al configurador (ver SmartPluginConfigurador) Se pasa el area como parámetro
            SmartPluginConfigurador form = new SmartGridPlugin.SmartPluginConfigurador(areaPoligono);
            form.ShowDialog();
            mostrarGridUI = form.MostrarGridUI;
            // Comprobamos si se ha elegido hacer división con recta o no
            divisionRecta = form.DivisionRecta;
            if (divisionRecta)
            {
                RectaUI formularioRecta = new RectaUI(Host.FPDrawnPolygon);
                formularioRecta.ShowDialog();
                listaPuntosPoligonos.Clear();
                listaPuntosPoligonos = formularioRecta.listaPuntosPoligonos;
                formularioRecta.Dispose();
            }
           
            if (divisionRecta)
            {
                for (int i = 0; i < 2; i++)
                {
                    // Dependiendo del tipo de división se llama a uno u otro método
                switch (form.TipoDivision)
                {
                    case 0: DivisionPoligono.divisionVertical(listaPuntosPoligonos[i], listaPoligonos, decimal.ToDouble(form.AreaMaxima), decimal.ToDouble(form.DesplazamientoMaximo));
                        break;
                    case 1: DivisionPoligono.divisionHorizontal(listaPuntosPoligonos[i], listaPoligonos, decimal.ToDouble(form.AreaMaxima), decimal.ToDouble(form.DesplazamientoMaximo));
                        break;
                    case 2: DivisionPoligono.divisionVertical(listaPuntosPoligonos[i], listaPoligonos, decimal.ToDouble(form.AreaMaxima), decimal.ToDouble(form.DesplazamientoMaximo), form.Franjas);
                        break;
                    case 3: DivisionPoligono.divisionHorizontal(listaPuntosPoligonos[i], listaPoligonos, decimal.ToDouble(form.AreaMaxima), decimal.ToDouble(form.DesplazamientoMaximo), form.Franjas);
                        break;
                }
                }
            }
            else
            {
                // Se crea una lista con los puntos del polígono original para que permanezca intacto
                List<PointLatLng> listaPuntosPoligonoInicial = new List<PointLatLng>();
                foreach (PointLatLng punto in Host.FPDrawnPolygon.Points)
                {
                    listaPuntosPoligonoInicial.Add(punto);
                }
                // Dependiendo del tipo de división se llama a uno u otro método
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
            }
            


            // llamar a una ventana que nos muestre un mapa con los polígonos generados

            VistaPrevia vistaPrevia = new VistaPrevia(listaPoligonos);
            vistaPrevia.ShowDialog();
            // Damos valor a la variable que controla el bucle
            divisionCorrecta = vistaPrevia.DivisionCorrecta;

            // Si la división es correcta, se guardan los parámetros por defecto de los grids:
            if (divisionCorrecta)
            {
                Host.config["grid_camera"] = form.Camara;
                Host.config["grid_alt"] = form.Altura.ToString();
                Host.config["grid_angle"] = form.Angulo.ToString();
                Host.config["grid_camdir"] = "True";
                Host.config["grid_dist"] = "10";
                Host.config["grid_overshoot1"] = form.OvershootHorizontal.ToString();
                Host.config["grid_overshoot2"] = form.OvershootVertical.ToString();
                Host.config["grid_overlap"] = form.Overlap.ToString();
                Host.config["grid_sidelap"] = form.Sidelap.ToString();
                Host.config["grid_startfrom"] = "BottomLeft";
                Host.config["grid_advanced"] = "True";
                nombreMision=form.NombreMision;
            }
            
            return divisionCorrecta;
        }

        /// <summary>
        /// Empleada al descargar el plugin
        /// </summary>
        /// <returns></returns>
        public override bool Exit()
        {
            return true;
        }
    }
}
