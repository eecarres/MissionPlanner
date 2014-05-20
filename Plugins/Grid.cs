using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MissionPlanner;
using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using MissionPlanner.Utilities;
using ProjNet.CoordinateSystems;
using ProjNet.CoordinateSystems.Transformations;


namespace SmartGridPlugin
{
    /// <summary>
    /// Clase que contiene la funcionalidad para crear la ruta del UAV
    /// </summary>
    public class Grid
    {   
        // Constantes para conversiones de grados decimales a radianes
        const float rad2deg = (float)(180 / Math.PI);
        const float deg2rad = (float)(1.0 / rad2deg);

        public struct linelatlng
        {
            // Punto inicial
            public utmpos p1;
            // Punto final
            public utmpos p2;
            // Utilizado como punto base de la recta para la ruta
            public utmpos basepnt;
        }

         // Las disintas opciones para el punto inicial de la ruta creada
        public enum StartPosition
        {
            Home = 0,
            BottomLeft = 1,
            TopLeft = 2,
            BottomRight = 3,
            TopRight = 4
        }

        // funciones sin uso actualmente
        static void addtomap(linelatlng pos)
        {
            return;
            //List<PointLatLng> list = new List<PointLatLng>();
            //list.Add(pos.p1.ToLLA());
            //list.Add(pos.p2.ToLLA());

         //   polygons.Routes.Add(new GMapRoute(list, "test") { Stroke = new System.Drawing.Pen(System.Drawing.Color.Yellow,4) });
            
            //.Markers.Add(new GMapMarkerGoogleRed(pnt));

            //map.ZoomAndCenterRoutes("polygons");

           // map.Invalidate();
        }
        static void addtomap(utmpos pos, string tag)
        {
            return;
            //tag = (no++).ToString();
          //  polygons.Markers.Add(new GMapMarkerGoogleRed(pos.ToLLA()));// { ToolTipText = tag, ToolTipMode = MarkerTooltipMode.Always } );

            //map.ZoomAndCenterMarkers("polygons");

            //map.Invalidate();
        }

        public static List<PointLatLngAlt> CreateGrid(List<PointLatLngAlt> polygon, double altitude, double distance, double spacing, double angle, double overshoot1,double overshoot2, StartPosition startpos, bool shutter)
        {
            // Ajusta el espaciado si es demasiado grande o si no existe
            if (spacing < 10 && spacing != 0)
                spacing = 10;

            if (polygon.Count == 0)
                return new List<PointLatLngAlt>();

            // Creamos la lista de puntos que se devolveran como Grid
            List<PointLatLngAlt> ans = new List<PointLatLngAlt>();

            //Sacamos la zona utm del primer punto del polígono
            int utmzone = polygon[0].GetUTMZone();

            // Creamos una lista de posiciones utmpos() que incluirá los puntos del polígono en formato utmpos,
            //a partir de los puntos del polígono y la posicion utm del primer punto del mismo
            
            List<utmpos> utmpositions = utmpos.ToList(PointLatLngAlt.ToUTM(utmzone, polygon), utmzone);

            //  Se hace coincidir al primer y ultimo punto del polígono
            if (utmpositions[0] != utmpositions[utmpositions.Count - 1])
                utmpositions.Add(utmpositions[0]); // make a full loop

            // Obtenemos el rectángulo formado con la máxima diagonal
            Rect area = getPolyMinMax(utmpositions);

            // Generamos la primera ruta

            // Obtenemos el valor de la diagonal para sacar el valor del área  aproximada del poligono original
            double diagdist = area.DiagDistance();

            // Lista para añadir las lineas que forman la ruta
            List<linelatlng> grid = new List<linelatlng>();
            // Lineas necesarias
            int lines = 0;
            // Empezamos en el punto medio del rectángulo
            double x = area.MidWidth;
            double y = area.MidHeight;
            
            
            // Obtenemos el punto a la izquierda (ver newpos)
            double xb1 = x;
            double yb1 = y;
            newpos(ref xb1, ref yb1, angle - 90, diagdist / 2 + distance);
            newpos(ref xb1, ref yb1, angle + 180, diagdist / 2 + distance);
            utmpos left = new utmpos(xb1, yb1, utmzone);


            // Obtenemos el punto a la derecha (ver newpos)
            double xb2 = x;
            double yb2 = y;
            newpos(ref xb2, ref yb2, angle + 90, diagdist / 2 + distance);
            newpos(ref xb2, ref yb2, angle + 180, diagdist / 2 + distance);
            utmpos right = new utmpos(xb2, yb2, utmzone);
            

            // Movemos la base al punto izquierdo
            x = xb1;
            y = yb1;

            // Dibujamos la cuadrícula inicial que cubre algo más del área del rectángulo
            while (lines < ((diagdist + distance * 2) / distance))
            {
                // El punto final será el inicial desplazado en sentido del rumbo
                double nx = x;
                double ny = y;
                newpos(ref nx, ref ny, angle, diagdist + distance*2);

                // Se crea la linea y se especifican sus puntos inicial final y base(inicial)
                linelatlng line = new linelatlng();
                line.p1 = new utmpos(x, y, utmzone);
                line.p2 = new utmpos(nx, ny, utmzone);
                line.basepnt = new utmpos(x, y, utmzone);
                // Se añade la linea como parte de la cuadricula
                grid.Add(line);

               //Movemos el punto inicial para hacer una línea paralela la próxima iteración
                newpos(ref x, ref y, angle + 90, distance);
                lines++;
            }

            // Encontramos intersecciones con nuestro polígono

            // Creamos la lista de rectas que no intersectan con el polígono
            List<linelatlng> remove = new List<linelatlng>();
            // Variable que controla el número de rectas que no cumplen la condicion
            int gridno = grid.Count;

            // Recorremos las rectas generadas
            for (int a = 0; a < gridno; a++)
            {
                // Damos valores extremos a los parámetros para identificar si cambian
                double closestdistance = double.MaxValue;
                double farestdistance = double.MinValue;

                utmpos closestpoint = utmpos.Zero;
                utmpos farestpoint = utmpos.Zero;

                // Lista con los puntos de intersección de cada recta
                List<utmpos> matchs = new List<utmpos>();

                int b = -1;
                int crosses = 0;
                utmpos newutmpos = utmpos.Zero;
                // Para cada uno de los puntos del polígono se evalúa 
                foreach (utmpos pnt in utmpositions)
                {
                    b++;
                    if (b == 0)
                    {
                        continue; // Ignoramos el primer punto ya que también se evalúa el último
                    }
                    // Buscamos la intersección de la recta creada del grid con cada una de las rectas del polígono
                    newutmpos = FindLineIntersection(utmpositions[b - 1], utmpositions[b], grid[a].p1, grid[a].p2);
                    // Si la línea NO es paralela
                    if (!newutmpos.IsZero)
                    {
                        crosses++; // Se suma una intersección
                        matchs.Add(newutmpos); // Añadimos el punto a la lista de puntos de intersección

                        // Buscamos si ese punto es el más cercano o más lejano a nuestra recta generada ( al punto inicial de la misma)
                        if (closestdistance > grid[a].p1.GetDistance(newutmpos)) 
                        {
                            closestpoint.y = newutmpos.y;
                            closestpoint.x = newutmpos.x;
                            closestpoint.zone = newutmpos.zone;
                            closestdistance = grid[a].p1.GetDistance(newutmpos); // Guardamos el punto, la zona UTM y la distancia
                        }
                        if (farestdistance < grid[a].p1.GetDistance(newutmpos))
                        {
                            farestpoint.y = newutmpos.y;
                            farestpoint.x = newutmpos.x;
                            farestpoint.zone = newutmpos.zone;
                            farestdistance = grid[a].p1.GetDistance(newutmpos); // Guardamos el punto, la zona UTM y la distancia
                        }
                    }
                }

                // Una vez tenemos todas las intersecciones de la línea generada actual con las rectas que forman el polígono

                if (crosses == 0) // Fuera del polígono, se guarda la recta generada en la lista de descartadas (lista remove)
                {
                    if (!PointInPolygon(grid[a].p1, utmpositions) && !PointInPolygon(grid[a].p2, utmpositions)) // Ninguno de los puntos está en el polígono
                        remove.Add(grid[a]); // Se añade a la lista de rectas descartadas
                }
                else if (crosses == 1) // No debería ocurrir, siempre debe tocar dos rectas como mínimo ( caso de linea tangente al poligono)
                {

                }
                else if (crosses == 2) // Caso típico, dos intersecciones de la recta con el poligono

                { // Se modifica la línea para que empiece y acabe en los puntos encontrados ( para que esté "contenida" en el polígono)
                    linelatlng line = grid[a];
                    line.p1 = closestpoint;
                    line.p2 = farestpoint;
                    grid[a] = line;
                }
                else // Caso de más de dos intersecciones ( polígonos con formas más complejas)
                {
                    linelatlng line = grid[a]; // Creamos una línea que es igual a la recta generada correspondiente a la iteración
                    remove.Add(line); // la añadimos  a la lista de descartes

                    while (matchs.Count > 1)
                    {
                        linelatlng newline = new linelatlng();

                        closestpoint = findClosestPoint(closestpoint, matchs); // Se busca el punto más cercano al closestpoint, entre los matchs
                        newline.p1 = closestpoint; // se fija el punto encontrado como el punto inicial de la recta
                        matchs.Remove(closestpoint); // Lo quitamos como match

                        closestpoint = findClosestPoint(closestpoint, matchs); // Se busca el punto más cercano al closestpoint que acabamos de definir, entre los matchs
                        newline.p2 = closestpoint; // se fija el punto encontrado como el punto final de la recta
                        matchs.Remove(closestpoint); // Lo quitamos como match

                        newline.basepnt = line.basepnt;

                        grid.Add(newline);  // Se añade la línea al grid
                    }
                }
            }

            foreach (linelatlng line in remove)
            {
                grid.Remove(line); // Eliminamos las líneas que hemos marcada como descartadas del grid
            }

            

            if (grid.Count == 0)
                return ans;

            // Pasamos ahora a situar nuestro punto inicial del grid. Para ello dependemos de la opción escogida por el usuario
            utmpos startposutm;

            switch (startpos)
            {
                    // dependiendo del caso, escogemos la posición de Home o una de las esquinas del rectángulo antes establecido
                default:
                case StartPosition.Home:
                    startposutm = new utmpos(GridPlugin.Host2.cs.HomeLocation);
                    break;
                case StartPosition.BottomLeft:
                    startposutm = new utmpos(area.Left, area.Bottom, utmzone);
                    break;
                case StartPosition.BottomRight:
                    startposutm = new utmpos(area.Right, area.Bottom, utmzone);
                    break;
                case StartPosition.TopLeft:
                    startposutm = new utmpos(area.Left, area.Top, utmzone);
                    break;
                case StartPosition.TopRight:
                    startposutm = new utmpos(area.Right, area.Top, utmzone);
                    break;

            }

            // Encontramos ahora la línea de entre las generadas que está mas cercana al punto determinado
            linelatlng closest = findClosestLine(startposutm, grid);

            utmpos lastpnt;

            if (closest.p1.GetDistance(startposutm) < closest.p2.GetDistance(startposutm))
            {
                lastpnt = closest.p1; // se guarda el punto inicial de la recta como punto final del grid
            }
            else
            {
                lastpnt = closest.p2;// se guarda el punto final de la recta como punto final del grid
            }
            
            // Mientras existan líneas en la lista del Grid
            while (grid.Count > 0)
            {
                if (closest.p1.GetDistance(lastpnt) < closest.p2.GetDistance(lastpnt)) // Se decide qué punto incluiremos a continuación: debe ser el más cercano al último punto definido
                {
                    ans.Add(closest.p1); // Añadimos el punto

                    if (spacing > 0) // Si hemos definido el espaciado como un valor no nulo (no puede ser negativo)
                    {
                        for (int d = (int)(spacing - ((closest.basepnt.GetDistance(closest.p1)) % spacing));
                            d < (closest.p1.GetDistance(closest.p2));
                            d += (int)spacing)
                        {
                            double ax = closest.p1.x;
                            double ay = closest.p1.y;

                            newpos(ref ax, ref ay, angle, d); // movemos el punto (ax,ay) en la direccion del rumbo
                            
                            ans.Add((new utmpos(ax, ay, utmzone) { Tag = "M" })); // añadimos los punto intermedios entre el punto inicial y final de cada recta
                        }
                    }


                    utmpos newend = newpos(closest.p2, angle, overshoot1); // Nos "pasamos" del punto final en un valor igual al overshoot
                  
                    
                    ans.Add(newend); // guardamos el punto creado por el overshoot

                    lastpnt = closest.p2; // El último punto pasa ahora a ser el punto final de la recta generada en la que estamos actualmente

                    grid.Remove(closest); // Eliminamos la recta de la lista Grid ( esto controla las iteraciones)
                    if (grid.Count == 0)  // Controlamos que queden rectas o se convertiria en una excepción
                        break;
                    closest = findClosestLine(newend, grid); // La linea de la nueva iteracion sera la más cercana al punto creado por el overshoot
                }
                else // Caso en el que el punto más cercano es el punto final de la primera recta, de modo que este es el punto inicial y los intermedios se definen con el rumbo opuesto
                {

                    ans.Add(closest.p2); // Añadimos el punto

                    if (spacing > 0)// Si hemos definido el espaciado como un valor no nulo (no puede ser negativo)
                    {
                        for (int d = (int)((closest.basepnt.GetDistance(closest.p2)) % spacing);
                            d < (closest.p1.GetDistance(closest.p2));
                            d += (int)spacing)
                        {
                            double ax = closest.p2.x;
                            double ay = closest.p2.y;

                            newpos(ref ax, ref ay, angle, -d); // movemos el punto (ax,ay) en la direccion del rumbo

                            ans.Add((new utmpos(ax, ay, utmzone) { Tag = "M" })); // añadimos los punto intermedios entre el punto inicial y final de cada recta
                        }
                    }

                    utmpos newend = newpos(closest.p1, angle, -overshoot2); // Nos "pasamos" del punto final en un valor igual al overshoot

                    ans.Add(newend); // guardamos el punto creado por el overshoot

                    lastpnt = closest.p1; // El último punto pasa ahora a ser el punto final de la recta generada en la que estamos actualmente

                    grid.Remove(closest); // Eliminamos la recta de la lista Grid ( esto controla las iteraciones)
                    if (grid.Count == 0) // Controlamos que queden rectas o se convertiria en una excepción
                        break;
                    closest = findClosestLine(newend, grid); // La linea de la nueva iteracion sera la más cercana al punto creado por el overshoot
                }
            }

            // Incluimos la altitud de cada punto de la solucion
            ans.ForEach(plla => { plla.Alt = altitude; });

            return ans;
        }


         // Funciones que sobran totalmente....
        /// <summary>
        /// Encuentra la ruta aplicando el algoritmo rapidly exploring random tree.
        /// http://en.wikipedia.org/wiki/Rapidly_exploring_random_tree
        /// </summary>
        /// <param name="grid1">The grid1.</param>
        /// <param name="startposutm">The startposutm.</param>
        /// <returns></returns>
        //static List<PointLatLngAlt> FindPath(List<linelatlng> grid1, utmpos startposutm)
        //{
        //    List<PointLatLngAlt> answer = new List<PointLatLngAlt>();

        //    List<linelatlng> closedset = new List<linelatlng>();
        //    List<linelatlng> openset = new List<linelatlng>(); // nodes to be travered
        //    Hashtable came_from = new Hashtable();
        //    List<linelatlng> grid = new List<linelatlng>();


        //    linelatlng start = new linelatlng() { p1 = startposutm, p2 = startposutm };

        //    grid.Add(start);
        //    grid.AddRange(grid1);
        //    openset.Add(start);

        //    Hashtable g_score = new Hashtable();
        //    Hashtable f_score = new Hashtable();
        //    g_score[start] = 0.0;
        //    f_score[start] = (double)g_score[start] + heuristic_cost_estimate(grid,0,start); // heuristic_cost_estimate(start, goal)

        //    linelatlng current = start;

        //    while (openset.Count > 0)
        //    {
        //        current = FindLowestFscore(g_score, openset); // lowest f_score
        //        openset.Remove(current);
        //        closedset.Add(current);
        //        foreach (var neighbor in neighbor_nodes(current, grid))
        //        {
        //            double tentative_g_score = (double)g_score[current];

        //            double dist1 = current.p1.GetDistance(neighbor.p1);
        //            double dist2 = current.p1.GetDistance(neighbor.p2);
        //            double dist3 = current.p2.GetDistance(neighbor.p1);
        //            double dist4 = current.p2.GetDistance(neighbor.p2);

        //            tentative_g_score += (dist1 + dist2 + dist3 + dist4) / 4;

        //            tentative_g_score  += neighbor.p1.GetDistance(neighbor.p2);

        //            //tentative_g_score += Math.Min(Math.Min(dist1, dist2), Math.Min(dist3, dist4));
        //            //tentative_g_score += Math.Max(Math.Max(dist1, dist2), Math.Max(dist3, dist4));

        //           // if (closedset.Contains(neighbor) && tentative_g_score >= (double)g_score[neighbor])
        //           //     continue;

        //            if (!closedset.Contains(neighbor) ||
        //               tentative_g_score < (double)g_score[neighbor])
        //            {
        //                came_from[neighbor] = current;
        //                g_score[neighbor] = tentative_g_score;
        //                f_score[neighbor] = tentative_g_score + heuristic_cost_estimate(grid, tentative_g_score, neighbor);
        //                Console.WriteLine("neighbor score: " + g_score[neighbor] + " " + f_score[neighbor]);
        //                if (!openset.Contains(neighbor))
        //                    openset.Add(neighbor);
        //            }
        //        }
               
        //    }

        //    // bad
        //    //linelatlng ans = FindLowestFscore(g_score, grid);

        //  //  foreach (var ans in grid)
        //    {
        //        List<linelatlng> list = reconstruct_path(came_from, current);
        //      //  list.Insert(0,current);
        //        //list.Remove(start);
        //        //list.Remove(start);
        //        Console.WriteLine("List " + list.Count + " " + g_score[current]);
        //       {
        //           List<utmpos> temp = new List<utmpos>();
        //           temp.Add(list[0].p1);
        //           temp.Add(list[0].p2);
        //           utmpos oldpos = findClosestPoint(startposutm, temp);

        //           foreach (var item in list) 
        //           {
        //               double dist1 = oldpos.GetDistance(item.p1);
        //               double dist2 = oldpos.GetDistance(item.p2);
        //               if (dist1 < dist2)
        //               {
        //                   answer.Add(new PointLatLngAlt(item.p1));
        //                   answer.Add(new PointLatLngAlt(item.p2));
        //                   oldpos = item.p2;
        //               }
        //               else
        //               {
        //                   answer.Add(new PointLatLngAlt(item.p2));
        //                   answer.Add(new PointLatLngAlt(item.p1));
        //                   oldpos = item.p1;
        //               }
        //           }
        //           //return answer;
        //       }
        //    }

        //    List<PointLatLng> list2 = new List<PointLatLng>();

        //    answer.ForEach(x => { list2.Add(x); });

        //    GMapPolygon wppoly = new GMapPolygon(list2, "Grid");

        //    Console.WriteLine("dist " + (wppoly.Distance));

        //    return answer;
        //}

        //static double heuristic_cost_estimate(List<linelatlng> grid, double sofar, linelatlng current_node)
        //{
        //    double ans = 0;

        //    linelatlng lastx = grid[0];

        //    grid.ForEach(x => 
        //    { 
        //        ans += x.p1.GetDistance(x.p2);
        //        ans += x.p1.GetDistance(lastx.p1);
        //        lastx = x; 
        //    });



        //    return ans - sofar * 0.95;
        //}

        //static List<linelatlng> reconstruct_path(Hashtable came_from, linelatlng current_node)
        //{
        //    List<linelatlng> ans = new List<linelatlng>();
        //    if (came_from.ContainsKey(current_node))
        //    {
                
        //        ans.AddRange(reconstruct_path(came_from, (linelatlng)came_from[current_node]));
        //        ans.Add((linelatlng)came_from[current_node]);
        //        return ans;
        //    }
        //    else
        //    {
        //        ans.Add(current_node);
        //        return ans;
        //    }
        //}


        //static private List<linelatlng> neighbor_nodes(linelatlng current, List<linelatlng> grid)
        //{
        //    List<linelatlng> neighbors = new List<linelatlng>();

        //    foreach (var item in grid)
        //    {
        //       // if (item.Equals(current))
        //      //      continue;

        //        neighbors.Add(item);
        //    }

        //    return neighbors;
        //}

        //static private linelatlng FindLowestFscore(Hashtable f_score, List<linelatlng> openset)
        //{
        //    linelatlng lowest = openset[0];
        //    int lowestint = int.MaxValue;

        //    foreach (linelatlng key in openset)
        //    {
        //        if (f_score.ContainsKey(key) && (double)f_score[key] < lowestint)
        //        {
        //            lowestint = (int)(double)f_score[key];
        //            lowest = key;
        //        }
        //    }

        //    Console.WriteLine("Lowest " + lowestint);

        //    return lowest;
        //}


        /// <summary>
        /// Devuelve un rectángulo que contiene al polígono de entrada
        /// </summary>
        /// <param name="utmpos">Lista de puntos del polígono de entrada</param>
        /// <returns></returns>
        static Rect getPolyMinMax(List<utmpos> utmpos)
        {
            if (utmpos.Count == 0)
                return new Rect();

            double minx, miny, maxx, maxy;

            minx = maxx = utmpos[0].x;
            miny = maxy = utmpos[0].y;

            foreach (utmpos pnt in utmpos)
            {
                minx = Math.Min(minx, pnt.x);
                maxx = Math.Max(maxx, pnt.x);

                miny = Math.Min(miny, pnt.y);
                maxy = Math.Max(maxy, pnt.y);
            }

            return new Rect(minx, maxy, maxx - minx,miny - maxy);
        }

        /// <summary>
        /// Mueve un punto a la nueva posición dado el punto inicial, la distancia y el rumbo
        /// </summary>
        /// <param name="x">Valor en X del punto, pasado por referencia</param>
        /// <param name="y">Valor en Y del punto, pasado por referencia</param>
        /// <param name="bearing">Rumbo</param>
        /// <param name="distance">Distancia</param>
        static void newpos(ref double x, ref double y, double bearing, double distance)
        {
            double degN = 90 - bearing;
            if (degN < 0)
                degN += 360;
            x = x + distance * Math.Cos(degN * deg2rad);
            y = y + distance * Math.Sin(degN * deg2rad);
        }

        /// <summary>
        /// Devuelve un punto con la nueva posición dado un punto inicial, la distancia y el rumbo
        /// </summary>
        /// <param name="input">Punto inicial</param>
        /// <param name="bearing">Rumbo</param>
        /// <param name="distance">Distancia a moverse</param>
        /// <returns></returns>
        static utmpos newpos(utmpos input, double bearing, double distance)
        {
            double degN = 90 - bearing;
            if (degN < 0)
                degN += 360;
            double x = input.x + distance * Math.Cos(degN * deg2rad);
            double y = input.y + distance * Math.Sin(degN * deg2rad);

            return new utmpos(x, y, input.zone);
        }

        /// <summary>
        /// from http://stackoverflow.com/questions/1119451/how-to-tell-if-a-line-intersects-a-polygon-in-c
        /// </summary>
        /// <param name="start1"></param>
        /// <param name="end1"></param>
        /// <param name="start2"></param>
        /// <param name="end2"></param>
        /// <returns></returns>
        public static utmpos FindLineIntersection(utmpos start1, utmpos end1, utmpos start2, utmpos end2)
        {
            double denom = ((end1.x - start1.x) * (end2.y - start2.y)) - ((end1.y - start1.y) * (end2.x - start2.x));
            //  Comprobamos si son paralelas        
            if (denom == 0)
                return utmpos.Zero;
            double numer = ((start1.y - start2.y) * (end2.x - start2.x)) - ((start1.x - start2.x) * (end2.y - start2.y));
            double r = numer / denom;
            double numer2 = ((start1.y - start2.y) * (end1.x - start1.x)) - ((start1.x - start2.x) * (end1.y - start1.y));
            double s = numer2 / denom;
            if ((r < 0 || r > 1) || (s < 0 || s > 1))
                return utmpos.Zero;
            // Encontramos el punto de intersección     
            utmpos result = new utmpos();
            result.x = start1.x + (r * (end1.x - start1.x));
            result.y = start1.y + (r * (end1.y - start1.y));
            result.zone = start1.zone;
            return result;
        }

        /// <summary>
        /// Encuentra el punto más cercano al indicado de una lista dada
        /// </summary>
        /// <param name="start">Punto al que nos referimos</param>
        /// <param name="list">Lista de puntos a comprobar</param>
        /// <returns></returns>
        static utmpos findClosestPoint(utmpos start, List<utmpos> list) 
        {
            utmpos answer = utmpos.Zero;
            double currentbest = double.MaxValue;

            foreach (utmpos pnt in list)
            {
                double dist1 = start.GetDistance(pnt); // Buscamos la distancia entre el punto referencia y el actual, si es menor al valor actual, este se actualiza

                if (dist1 < currentbest)
                {
                    answer = pnt;
                    currentbest = dist1;
                }
            }

            return answer;
        }

        /// <summary>
        /// Encuentra la línea más cercana al punto indicado de una lista dada
        /// </summary>
        /// <param name="start">Punto al que nos referimos</param>
        /// <param name="list">Lista de rectas a comprobar</param>
        /// <returns></returns>
        static linelatlng findClosestLine(utmpos start, List<linelatlng> list)
        {
            linelatlng answer = list[0];
            double shortest = double.MaxValue;

            foreach (linelatlng line in list)
            {
                double ans1 = start.GetDistance(line.p1);
                double ans2 = start.GetDistance(line.p2);
                utmpos shorterpnt = ans1 < ans2 ? line.p1 : line.p2;

                if (shortest > start.GetDistance(shorterpnt))
                {
                    answer = line;
                    shortest = start.GetDistance(shorterpnt);
                }
            }

            return answer;
        }

        /// <summary>
        /// Método que devuelve true si el punto está contenido en el polígono o viceversa
        /// </summary>
        /// <param name="p">El punto a comprobar</param>
        /// <param name="poly">El polígono que hay que comprobar</param>
        /// <returns></returns>
        static bool PointInPolygon(utmpos p, List<utmpos> poly)
        {
            utmpos p1, p2;
            bool inside = false;

            if (poly.Count < 3)
            {
                return inside;
            }
            utmpos oldPoint = new utmpos(poly[poly.Count - 1]);

            for (int i = 0; i < poly.Count; i++)
            {

                utmpos newPoint = new utmpos(poly[i]);

                if (newPoint.y > oldPoint.y)
                {
                    p1 = oldPoint;
                    p2 = newPoint;
                }
                else
                {
                    p1 = newPoint;
                    p2 = oldPoint;
                }

                if ((newPoint.y < p.y) == (p.y <= oldPoint.y)
                    && ((double)p.x - (double)p1.x) * (double)(p2.y - p1.y)
                    < ((double)p2.x - (double)p1.x) * (double)(p.y - p1.y))
                {
                    inside = !inside;
                }
                oldPoint = newPoint;
            }
            return inside;
        }

    }
}
