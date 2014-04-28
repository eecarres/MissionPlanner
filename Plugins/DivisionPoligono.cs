﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GMap.NET;
using  GMap.NET.WindowsForms;
using GMap.NET.Internals;
using GMap.NET.WindowsForms.Markers;
using System.Drawing;
using MissionPlanner.Utilities;
using GeoUtility;

namespace SmartGridPlugin
{
    class DivisionPoligono
    {
        public static  int iteraciones = 0; // Control de las iteraciones del área
        public static List<PointLatLng> listaPoligonoGenerado = new List<PointLatLng>();
        public static List<PointLatLng> listaPoligonoRestante = new List<PointLatLng>();
        public static List<PointLatLng> listaPoligonoRestanteTemporal = new List<PointLatLng>();
        public static List<PointLatLng> listaPoligonoGeneradoTemporal = new List<PointLatLng>();
        public static  List<Recta> listaRectas = new List<Recta>();

        // La llamada desde el formulario debe incluir el polígono dibujado y la lista de los polígonos del mapa
        public static void divisionVertical(List<PointLatLng> puntosPoligono, List<GMapPolygon> listaPoligono, double areaMaxima, double desplazamientoMaximo)
        {
            //Creamos la lista de puntos UTM del polígono
            List<double[]> puntosUTMPoligono = new List<double[]>();
            List<PointLatLng> puntosWGSPoligono = new List<PointLatLng>();
            // Algoritmo que reordena los puntos. El primero, el de la izquierda del todo, luego sentido horario


            int utmzone = Utilidades.ZonaUtm(puntosPoligono[0]); // Zona UTM del primer punto definido
            for (int i = 0; i < puntosPoligono.Count; i++)
            {
                puntosUTMPoligono.Add(Utilidades.PasarACartesianas(puntosPoligono[i], utmzone)); // Se añade el punto UTM de cada punto LatLng del polígono
            }

            // Sacamos el índice del punto más al sur
            int puntoInicio = 0;
            double valorAComprobar = puntosUTMPoligono[0][0];
            for (int i = 0; i < puntosUTMPoligono.Count; i++)
            {
                if (puntosUTMPoligono[i][0] < valorAComprobar) // Comprobamos si está a debajo de nuestro punto por defecto
                {
                    valorAComprobar = puntosUTMPoligono[i][0];
                    puntoInicio = i;
                }
            }

            // Reordenamos el List de puntos WGS con los UTM y el valor del punto más a la izquierda
            int x = 0;
            int y = 0;
            for (int i = 0; i < puntosUTMPoligono.Count; i++)
            {
                if (puntoInicio + i < puntosUTMPoligono.Count)
                {
                    x = puntoInicio + i;
                    puntosWGSPoligono.Add(PointLatLngAlt.FromUTM(utmzone, puntosUTMPoligono[x][0], puntosUTMPoligono[x][1]));
                }
                else
                {

                    puntosWGSPoligono.Add(PointLatLngAlt.FromUTM(utmzone, puntosUTMPoligono[y][0], puntosUTMPoligono[y][1]));
                    y++;
                }
            }



            for (int i = 0; i < puntosWGSPoligono.Count; i++) // Ahora ordenamos los puntos UTM con los WGS ya ordenados (pasando coordenadas)
            {
                puntosUTMPoligono[i] = Utilidades.PasarACartesianas(puntosWGSPoligono[i], utmzone);
            }
            puntosUTMPoligono.Add(puntosUTMPoligono[0]); // HAcemos full loop de los puntos (último y primero son los mismos)

            // Asignamos los valores de las propiedades del polígono y de la operación de division de areas

            double areaCalculada = 0; // Controla el área que calculamos en cada iteración
            // Empezamos diciendo que máximo queremos 30 hectáreas
            double[] puntoControl = { 0.0, 0.0 }; // El punto que iremos moviendo para dibujar la línea vertical


            // Se calcula el valor que debe tener cada parcela aproximadamente dado un valor máximo (en el formulario)
            double areaPoligonoTotal = Utilidades.calcpolygonarea(puntosWGSPoligono) / 10000;
            double numeroDePoligonos = areaPoligonoTotal / areaMaxima;
            numeroDePoligonos = Math.Truncate(numeroDePoligonos) + 1;
            areaMaxima = areaPoligonoTotal / numeroDePoligonos;





            for (int i = 0; i < numeroDePoligonos - 1; i++) // el -1 es porque la última vez no hay que hacerlo (el último polígono es el restante)
            {
                areaCalculada = 0;
                iteraciones = 0;






                if (i == 0) // La primera vez, el polígono es el que habíamos dibujado y entra como referencia
                {
                    SacaRectas(puntosUTMPoligono, utmzone);
                    while (areaCalculada < areaMaxima)
                    {
                        areaCalculada = CalculaAreaVertical(puntosUTMPoligono, listaRectas, utmzone, puntoControl, desplazamientoMaximo);
                    }


                }
                else // para el resto de ocasiones, el área se calcula con el polígono restante que hemos obtenido en la iteración anterior.
                {
                    List<double[]> poligonoUTMRestante = new List<double[]>();
                    List<PointLatLngAlt> conversor = new List<PointLatLngAlt>();
                    for (int j = 0; j < listaPoligonoRestante.Count; j++)
                    {
                        conversor.Add(listaPoligonoRestante[j]);
                    }
                    poligonoUTMRestante = PointLatLngAlt.ToUTM(utmzone, conversor);
                    SacaRectas(listaPoligonoRestante); // Sacamos las rectas del polígono a dividir
                    while (areaCalculada < areaMaxima)
                    {
                        areaCalculada = CalculaAreaVertical(poligonoUTMRestante, listaRectas, utmzone, puntoControl, desplazamientoMaximo);
                    }
                    //MessageBox.Show((i+1)+" Área calculada. Valor: " + areaCalculada);
                }

                // Se generan los dos polígonos y se asignan valores para sus representacione gráficas
                GMapPolygon poligonoGenerado = new GMapPolygon(listaPoligonoGenerado, "generado " + i);
                GMapPolygon poligonoRestante = new GMapPolygon(listaPoligonoRestante, "restante");
                poligonoGenerado.Fill = new SolidBrush(Color.FromArgb(6, Color.DarkRed));
                poligonoGenerado.Stroke = new Pen(Color.Black, 2);
                poligonoRestante.Fill = new SolidBrush(Color.FromArgb(6, Color.DarkRed));
                poligonoRestante.Stroke = new Pen(Color.Black, 2);

                // Se añaden los polígonos a la lista
                listaPoligono.Add(poligonoGenerado);
                if (i == numeroDePoligonos - 2)
                {
                    listaPoligono.Add(poligonoRestante);
                }


            }

        }

        public static void divisionVertical(List<PointLatLng> puntosPoligono, List<GMapPolygon> listaPoligono, double areaMaxima, double desplazamientoMaximo, int franjas)
        {
            //Creamos la lista de puntos UTM del polígono
            List<double[]> puntosUTMPoligono = new List<double[]>();
            List<PointLatLng> puntosWGSPoligono = new List<PointLatLng>();
            double areaFranjas = areaMaxima;
            // Algoritmo que reordena los puntos. El primero, el de la izquierda del todo, luego sentido horario


            int utmzone = Utilidades.ZonaUtm(puntosPoligono[0]); // Zona UTM del primer punto definido
            for (int i = 0; i < puntosPoligono.Count; i++)
            {
                puntosUTMPoligono.Add(Utilidades.PasarACartesianas(puntosPoligono[i], utmzone)); // Se añade el punto UTM de cada punto LatLng del polígono
            }

            // Sacamos el índice del punto 
            int puntoInicio = 0;
            double valorAComprobar = puntosUTMPoligono[0][0];
            for (int i = 0; i < puntosUTMPoligono.Count; i++)
            {
                if (puntosUTMPoligono[i][0] < valorAComprobar) // Comprobamos si está a la izquierda de nuestro punto por defecto
                {
                    valorAComprobar = puntosUTMPoligono[i][0];
                    puntoInicio = i;
                }
            }

            // Reordenamos el List de puntos WGS con los UTM y el valor del punto más a la izquierda
            int x = 0;
            int y = 0;
            for (int i = 0; i < puntosUTMPoligono.Count; i++)
            {
                if (puntoInicio + i < puntosUTMPoligono.Count)
                {
                    x = puntoInicio + i;
                    puntosWGSPoligono.Add(PointLatLngAlt.FromUTM(utmzone, puntosUTMPoligono[x][0], puntosUTMPoligono[x][1]));
                }
                else
                {

                    puntosWGSPoligono.Add(PointLatLngAlt.FromUTM(utmzone, puntosUTMPoligono[y][0], puntosUTMPoligono[y][1]));
                    y++;
                }
            }



            for (int i = 0; i < puntosWGSPoligono.Count; i++) // Ahora ordenamos los puntos UTM con los WGS ya ordenados (pasando coordenadas)
            {
                puntosUTMPoligono[i] = Utilidades.PasarACartesianas(puntosWGSPoligono[i], utmzone);
            }
            puntosUTMPoligono.Add(puntosUTMPoligono[0]); // HAcemos full loop de los puntos (último y primero son los mismos)

            // Asignamos los valores de las propiedades del polígono y de la operación de division de areas

            double areaCalculada = 0; // Controla el área que calculamos en cada iteración
            // Empezamos diciendo que máximo queremos 30 hectáreas
            double[] puntoControl = { 0.0, 0.0 }; // El punto que iremos moviendo para dibujar la línea vertical


            // Se calcula el valor que debe tener cada parcela aproximadamente dado un valor máximo (en el formulario)
            double areaPoligonoTotal = Utilidades.calcpolygonarea(puntosWGSPoligono) / 10000;
            areaMaxima = areaMaxima * franjas;
            double numeroDePoligonos = areaPoligonoTotal / areaMaxima;
            numeroDePoligonos = Math.Truncate(numeroDePoligonos) + 1;
            areaMaxima = areaPoligonoTotal / numeroDePoligonos;

            // Solo para MIXTO

            for (int i = 0; i < numeroDePoligonos - 1; i++) // el -1 es porque la última vez no hay que hacerlo (el último polígono es el restante)
            {
                areaCalculada = 0;
                iteraciones = 0;

                if (i == 0) // La primera vez, el polígono es el que habíamos dibujado y entra como referencia
                {
                    SacaRectas(puntosUTMPoligono, utmzone);
                    while (areaCalculada < areaMaxima)
                    {
                        areaCalculada = CalculaAreaVerticalFranja(puntosUTMPoligono, listaRectas, utmzone, puntoControl, desplazamientoMaximo);
                    }

                    //MessageBox.Show("Primera área calculada. Valor: " + areaCalculada+" Num iteraciones: "+iteraciones);
                }
                else // para el resto de ocasiones, el área se calcula con el polígono restante que hemos obtenido en la iteración anterior.
                {
                    List<double[]> poligonoUTMRestante = new List<double[]>();
                    List<PointLatLngAlt> conversor = new List<PointLatLngAlt>();
                    for (int j = 0; j < listaPoligonoRestanteTemporal.Count; j++)
                    {
                        conversor.Add(listaPoligonoRestanteTemporal[j]);
                    }
                    poligonoUTMRestante = PointLatLngAlt.ToUTM(utmzone, conversor);
                    SacaRectas(listaPoligonoRestanteTemporal); // Sacamos las rectas del polígono a dividir
                    while (areaCalculada < areaMaxima)
                    {
                        areaCalculada = CalculaAreaVerticalFranja(poligonoUTMRestante, listaRectas, utmzone, puntoControl, desplazamientoMaximo);
                    }
                    //MessageBox.Show((i+1)+" Área calculada. Valor: " + areaCalculada);
                }

                // Se generan los dos polígonos y se asignan valores para sus representacione gráficas
                
                
                //Se divide el polígono generado y se guarda en la lista "buena" de poligonos
                divisionHorizontal(listaPoligonoGeneradoTemporal, listaPoligono,areaFranjas, desplazamientoMaximo);

                // Cuando llegamos al penúltimo poligono, se divide también el poligono restante
                if (i==numeroDePoligonos-2)
                {
                 divisionHorizontal(listaPoligonoRestanteTemporal, listaPoligono, areaFranjas, desplazamientoMaximo);  
                }
                
                

            }

 

        }

        public static void divisionHorizontal(List<PointLatLng> puntosPoligono, List<GMapPolygon> listaPoligono, double areaMaxima, double desplazamientoMaximo)
        {
            //Creamos la lista de puntos UTM del polígono
            List<double[]> puntosUTMPoligono = new List<double[]>();
            List<PointLatLng> puntosWGSPoligono = new List<PointLatLng>();
            // Algoritmo que reordena los puntos. El primero, el de la izquierda del todo, luego sentido horario


            int utmzone = Utilidades.ZonaUtm(puntosPoligono[0]); // Zona UTM del primer punto definido
            for (int i = 0; i < puntosPoligono.Count; i++)
            {
                puntosUTMPoligono.Add(Utilidades.PasarACartesianas(puntosPoligono[i], utmzone)); // Se añade el punto UTM de cada punto LatLng del polígono
            }

            // Sacamos el índice del punto más al sur
            int puntoInicio = 0;
            double valorAComprobar = puntosUTMPoligono[0][1];
            for (int i = 0; i < puntosUTMPoligono.Count; i++)
            {
                if (puntosUTMPoligono[i][1] < valorAComprobar) // Comprobamos si está a debajo de nuestro punto por defecto
                {
                    valorAComprobar = puntosUTMPoligono[i][1];
                    puntoInicio = i;
                }
            }

            // Reordenamos el List de puntos WGS con los UTM y el valor del punto más a la izquierda
            int x = 0;
            int y = 0;
            for (int i = 0; i < puntosUTMPoligono.Count; i++)
            {
                if (puntoInicio + i < puntosUTMPoligono.Count)
                {
                    x = puntoInicio + i;
                    puntosWGSPoligono.Add(PointLatLngAlt.FromUTM(utmzone, puntosUTMPoligono[x][0], puntosUTMPoligono[x][1]));
                }
                else
                {

                    puntosWGSPoligono.Add(PointLatLngAlt.FromUTM(utmzone, puntosUTMPoligono[y][0], puntosUTMPoligono[y][1]));
                    y++;
                }
            }



            for (int i = 0; i < puntosWGSPoligono.Count; i++) // Ahora ordenamos los puntos UTM con los WGS ya ordenados (pasando coordenadas)
            {
                puntosUTMPoligono[i] = Utilidades.PasarACartesianas(puntosWGSPoligono[i], utmzone);
            }
            puntosUTMPoligono.Add(puntosUTMPoligono[0]); // HAcemos full loop de los puntos (último y primero son los mismos)

            // Asignamos los valores de las propiedades del polígono y de la operación de division de areas

            double areaCalculada = 0; // Controla el área que calculamos en cada iteración
            // Empezamos diciendo que máximo queremos 30 hectáreas
            double[] puntoControl = { 0.0, 0.0 }; // El punto que iremos moviendo para dibujar la línea vertical


            // Se calcula el valor que debe tener cada parcela aproximadamente dado un valor máximo (en el formulario)
            double areaPoligonoTotal = Utilidades.calcpolygonarea(puntosWGSPoligono) / 10000;
            double numeroDePoligonos = areaPoligonoTotal / areaMaxima;
            numeroDePoligonos = Math.Truncate(numeroDePoligonos) + 1;
            areaMaxima = areaPoligonoTotal / numeroDePoligonos;





            for (int i = 0; i < numeroDePoligonos - 1; i++) // el -1 es porque la última vez no hay que hacerlo (el último polígono es el restante)
            {
                areaCalculada = 0;
                iteraciones = 0;
               
                




                if (i == 0) // La primera vez, el polígono es el que habíamos dibujado y entra como referencia
                {
                    SacaRectas(puntosUTMPoligono, utmzone);
                    while (areaCalculada < areaMaxima)
                    {
                        areaCalculada = CalculaAreaHorizontal(puntosUTMPoligono, listaRectas, utmzone, puntoControl, desplazamientoMaximo);
                    }

                   
                }
                else // para el resto de ocasiones, el área se calcula con el polígono restante que hemos obtenido en la iteración anterior.
                {
                    List<double[]> poligonoUTMRestante = new List<double[]>();
                    List<PointLatLngAlt> conversor = new List<PointLatLngAlt>();
                    for (int j = 0; j < listaPoligonoRestante.Count; j++)
                    {
                        conversor.Add(listaPoligonoRestante[j]);
                    }
                    poligonoUTMRestante = PointLatLngAlt.ToUTM(utmzone, conversor);
                    SacaRectas(listaPoligonoRestante); // Sacamos las rectas del polígono a dividir
                    while (areaCalculada < areaMaxima)
                    {
                        areaCalculada = CalculaAreaHorizontal(poligonoUTMRestante, listaRectas, utmzone, puntoControl, desplazamientoMaximo);
                    }
                    //MessageBox.Show((i+1)+" Área calculada. Valor: " + areaCalculada);
                }

                // Se generan los dos polígonos y se asignan valores para sus representacione gráficas
                GMapPolygon poligonoGenerado = new GMapPolygon(listaPoligonoGenerado, "generado " + i);
                GMapPolygon poligonoRestante = new GMapPolygon(listaPoligonoRestante, "restante");
                poligonoGenerado.Fill = new SolidBrush(Color.FromArgb(6, Color.DarkRed));
                poligonoGenerado.Stroke = new Pen(Color.Black, 2);
                poligonoRestante.Fill = new SolidBrush(Color.FromArgb(6, Color.DarkRed));
                poligonoRestante.Stroke = new Pen(Color.Black, 2);

                // Se añaden los polígonos a la lista
                listaPoligono.Add(poligonoGenerado);
                if (i==numeroDePoligonos-2)
                {
                    listaPoligono.Add(poligonoRestante);
                }
               

            }

        }

        public static void divisionHorizontal(List<PointLatLng> puntosPoligono, List<GMapPolygon> listaPoligono, double areaMaxima, double desplazamientoMaximo, int franjas)
        {
            //Creamos la lista de puntos UTM del polígono
            List<double[]> puntosUTMPoligono = new List<double[]>();
            List<PointLatLng> puntosWGSPoligono = new List<PointLatLng>();
            double areaFranjas = areaMaxima;
            // Algoritmo que reordena los puntos. El primero, el de la izquierda del todo, luego sentido horario


            int utmzone = Utilidades.ZonaUtm(puntosPoligono[0]); // Zona UTM del primer punto definido
            for (int i = 0; i < puntosPoligono.Count; i++)
            {
                puntosUTMPoligono.Add(Utilidades.PasarACartesianas(puntosPoligono[i], utmzone)); // Se añade el punto UTM de cada punto LatLng del polígono
            }

            // Sacamos el índice del punto 
            int puntoInicio = 0;
            double valorAComprobar = puntosUTMPoligono[0][1];
            for (int i = 0; i < puntosUTMPoligono.Count; i++)
            {
                if (puntosUTMPoligono[i][1] < valorAComprobar) // Comprobamos si está a la izquierda de nuestro punto por defecto
                {
                    valorAComprobar = puntosUTMPoligono[i][1];
                    puntoInicio = i;
                }
            }

            // Reordenamos el List de puntos WGS con los UTM y el valor del punto más a la izquierda
            int x = 0;
            int y = 0;
            for (int i = 0; i < puntosUTMPoligono.Count; i++)
            {
                if (puntoInicio + i < puntosUTMPoligono.Count)
                {
                    x = puntoInicio + i;
                    puntosWGSPoligono.Add(PointLatLngAlt.FromUTM(utmzone, puntosUTMPoligono[x][0], puntosUTMPoligono[x][1]));
                }
                else
                {

                    puntosWGSPoligono.Add(PointLatLngAlt.FromUTM(utmzone, puntosUTMPoligono[y][0], puntosUTMPoligono[y][1]));
                    y++;
                }
            }



            for (int i = 0; i < puntosWGSPoligono.Count; i++) // Ahora ordenamos los puntos UTM con los WGS ya ordenados (pasando coordenadas)
            {
                puntosUTMPoligono[i] = Utilidades.PasarACartesianas(puntosWGSPoligono[i], utmzone);
            }
            puntosUTMPoligono.Add(puntosUTMPoligono[0]); // HAcemos full loop de los puntos (último y primero son los mismos)

            // Asignamos los valores de las propiedades del polígono y de la operación de division de areas

            double areaCalculada = 0; // Controla el área que calculamos en cada iteración
            // Empezamos diciendo que máximo queremos 30 hectáreas
            double[] puntoControl = { 0.0, 0.0 }; // El punto que iremos moviendo para dibujar la línea vertical


            // Se calcula el valor que debe tener cada parcela aproximadamente dado un valor máximo (en el formulario)
            double areaPoligonoTotal = Utilidades.calcpolygonarea(puntosWGSPoligono) / 10000;
            areaMaxima = areaMaxima * franjas;
            double numeroDePoligonos = areaPoligonoTotal / areaMaxima;
            numeroDePoligonos = Math.Truncate(numeroDePoligonos) + 1;
            areaMaxima = areaPoligonoTotal / numeroDePoligonos;

            // Solo para MIXTO

            for (int i = 0; i < numeroDePoligonos - 1; i++) // el -1 es porque la última vez no hay que hacerlo (el último polígono es el restante)
            {
                areaCalculada = 0;
                iteraciones = 0;

                if (i == 0) // La primera vez, el polígono es el que habíamos dibujado y entra como referencia
                {
                    SacaRectas(puntosUTMPoligono, utmzone);
                    while (areaCalculada < areaMaxima)
                    {
                        areaCalculada = CalculaAreaHorizontalFranja(puntosUTMPoligono, listaRectas, utmzone, puntoControl, desplazamientoMaximo);
                    }

                    //MessageBox.Show("Primera área calculada. Valor: " + areaCalculada+" Num iteraciones: "+iteraciones);
                }
                else // para el resto de ocasiones, el área se calcula con el polígono restante que hemos obtenido en la iteración anterior.
                {
                    List<double[]> poligonoUTMRestante = new List<double[]>();
                    List<PointLatLngAlt> conversor = new List<PointLatLngAlt>();
                    for (int j = 0; j < listaPoligonoRestanteTemporal.Count; j++)
                    {
                        conversor.Add(listaPoligonoRestanteTemporal[j]);
                    }
                    poligonoUTMRestante = PointLatLngAlt.ToUTM(utmzone, conversor);
                    SacaRectas(listaPoligonoRestanteTemporal); // Sacamos las rectas del polígono a dividir
                    while (areaCalculada < areaMaxima)
                    {
                        areaCalculada = CalculaAreaHorizontalFranja(poligonoUTMRestante, listaRectas, utmzone, puntoControl, desplazamientoMaximo);
                    }
                    //MessageBox.Show((i+1)+" Área calculada. Valor: " + areaCalculada);
                }

                // Se generan los dos polígonos y se asignan valores para sus representacione gráficas


                //Se divide el polígono generado y se guarda en la lista "buena" de poligonos
                divisionVertical(listaPoligonoGeneradoTemporal, listaPoligono, areaFranjas, desplazamientoMaximo);

                // Cuando llegamos al penúltimo poligono, se divide también el poligono restante
                if (i == numeroDePoligonos - 2)
                {
                    divisionVertical(listaPoligonoRestanteTemporal, listaPoligono, areaFranjas, desplazamientoMaximo);
                }



            }



        }

        // Función que calcula cuanto hay que mover la línea, dado el valor de distancia en X de los dos puntos de la recta que recorremos
        public static double IncrementoLineal(double distanciaEntrePuntos)
        {
            double resultado;
            double distanciaEntreLineasMaxima = 0.5; // se establece un valor de 200 metros de distancia máxima entre líneas
            double numeroDeIteraciones = distanciaEntrePuntos / distanciaEntreLineasMaxima;
            numeroDeIteraciones = Math.Truncate(numeroDeIteraciones)+1;
           resultado= distanciaEntrePuntos / numeroDeIteraciones;
           return resultado;
        
        }

        // Funcion que calcula el area dado el poligono 
        public static double CalculaAreaVertical(List<double[]> puntosUTMPoligono, List<Recta> listaRectas, int utmzone,double[] puntoControl,double incrementoMax)
        { 
        double resultado;
        
        
        iteraciones++;
         List<Recta> rectasCortadas=new List<Recta>();
         List<Recta> rectasIncluidas= new List<Recta>();
         List<Recta> rectasIrrelevantes = new List<Recta>();

         List<double[]> poligonoUTMGenerado = new List<double[]>();
         List<double[]> poligonoUTMRestante = new List<double[]>();

         listaPoligonoGenerado.Clear();
         listaPoligonoRestante.Clear();


         int rectaCorteSuperior=99, rectaCorteInferior = 99; // Los identificadores de la posicion de las rectas de corte

            // Si es la primera recta, el punto de control es el primer punto del poligono (siempre a la izquierda)
         if (iteraciones == 1)
         {
             puntoControl[0] = puntosUTMPoligono[0][0];
             puntoControl[1] = puntosUTMPoligono[0][1];
         }
         

            // Incrementamos la dirección X en un incrementoLineal
        double incremento = IncrementoLineal(incrementoMax);
        puntoControl[0]+=incremento;
        

            // Generamos una recta vertical a una distancia igual al incrementoLineal
            
            
            // Evaluamos los puntos de cada recta, donde quedan en relacion a la recta vertical (izquierda o derecha) y añadimos los puntos al poligono generado y al restante
            // Primero el punto que seguro que está a la izquierda, el primero del poligono.
            poligonoUTMGenerado.Add(puntosUTMPoligono[0]);

           
            for (int i = 0; i < listaRectas.Count; i++)
            {

                // Si los dos puntos estan a la izquierda (Y es menor)
                if (listaRectas[i].punto1[0] < puntoControl[0] && listaRectas[i].punto2[0] < puntoControl[0])
                {
                    rectasIncluidas.Add(listaRectas[i]);
                    poligonoUTMGenerado.Add(listaRectas[i].punto2);
                }
                // Si uno dos puntos esta a la izquierda (Y es menor) y el otro a la derecha 
                else if (listaRectas[i].punto1[0] < puntoControl[0] && listaRectas[i].punto2[0] > puntoControl[0] )
                {
                    rectasCortadas.Add(listaRectas[i]);
                    rectaCorteSuperior = i;

                    poligonoUTMGenerado.Add(Utilidades.InterseccionVertical(listaRectas[i],puntoControl));
                    poligonoUTMRestante.Add(Utilidades.InterseccionVertical(listaRectas[i], puntoControl));
                    poligonoUTMRestante.Add(listaRectas[i].punto2);
                }
                // o al revés
                else if (listaRectas[i].punto1[0] > puntoControl[0] && listaRectas[i].punto2[0] < puntoControl[0])
	                {
                 rectasCortadas.Add(listaRectas[i]);
                 rectaCorteInferior = i;
                    poligonoUTMGenerado.Add(Utilidades.InterseccionVertical(listaRectas[i],puntoControl));
                    poligonoUTMGenerado.Add(listaRectas[i].punto2);
                    poligonoUTMRestante.Add(Utilidades.InterseccionVertical(listaRectas[i], puntoControl));
	                }
                  
                // Si los dos puntos estan a la derecha (Y es mayor)
                else if (listaRectas[i].punto1[0] > puntoControl[0] && listaRectas[i].punto2[0] > puntoControl[0])
                {
                    rectasIrrelevantes.Add(listaRectas[i]);
                    poligonoUTMRestante.Add(listaRectas[i].punto2);
                }
               
            }

            

            // Con los dos poligonos ya definidos en UTM, se pasan a LatLng y se calcula su área (en ha)

            for (int i = 0; i < poligonoUTMGenerado.Count; i++)
            {
                PointLatLng control=new PointLatLng();
                control=Utilidades.PasarAWGS(poligonoUTMGenerado[i],utmzone);
                listaPoligonoGenerado.Add(control);
            }
            for (int i = 0; i < poligonoUTMRestante.Count; i++)
            {
                PointLatLng control = new PointLatLng();
                control = Utilidades.PasarAWGS(poligonoUTMRestante[i], utmzone);
                listaPoligonoRestante.Add(control);
            }

            resultado = Utilidades.calcpolygonarea(listaPoligonoGenerado)/10000;
           

            return resultado;
        }
        
        public static double CalculaAreaHorizontal(List<double[]> puntosUTMPoligono, List<Recta> listaRectas, int utmzone,double[] puntoControl,double incrementoMax)
        { 
        double resultado;
        
        
        iteraciones++;
         List<Recta> rectasCortadas=new List<Recta>();
         List<Recta> rectasIncluidas= new List<Recta>();
         List<Recta> rectasIrrelevantes = new List<Recta>();

         List<double[]> poligonoUTMGenerado = new List<double[]>();
         List<double[]> poligonoUTMRestante = new List<double[]>();

         listaPoligonoGenerado.Clear();
         listaPoligonoRestante.Clear();


         int rectaCorteSuperior=99, rectaCorteInferior = 99; // Los identificadores de la posicion de las rectas de corte

            // Si es la primera recta, el punto de control es el primer punto del poligono
         if (iteraciones == 1)
         {
             puntoControl[0] = puntosUTMPoligono[0][0];
             puntoControl[1] = puntosUTMPoligono[0][1];
         }
         

            // Incrementamos la dirección X en un incrementoLineal
        double incremento = IncrementoLineal(incrementoMax);
        puntoControl[1]+=incremento;
        
            // Evaluamos los puntos de cada recta, donde quedan en relacion a la recta vertical (izquierda o derecha) y añadimos los puntos al poligono generado y al restante
            // Primero el punto que seguro que está a la izquierda, el primero del poligono.
            poligonoUTMGenerado.Add(puntosUTMPoligono[0]);

           
            for (int i = 0; i < listaRectas.Count; i++)
            {

                // Si los dos puntos estan a la izquierda (Y es menor)
                if (listaRectas[i].punto1[1] < puntoControl[1] && listaRectas[i].punto2[1] < puntoControl[1])
                {
                    rectasIncluidas.Add(listaRectas[i]);
                    poligonoUTMGenerado.Add(listaRectas[i].punto2);
                }
                // Si uno dos puntos esta a la izquierda (Y es menor) y el otro a la derecha o al revés
                else if (listaRectas[i].punto1[1] < puntoControl[1] && listaRectas[i].punto2[1] > puntoControl[1] )
                {
                    rectasCortadas.Add(listaRectas[i]);
                    rectaCorteSuperior = i;

                    poligonoUTMGenerado.Add(Utilidades.InterseccionHorizontal(listaRectas[i],puntoControl));
                    poligonoUTMRestante.Add(Utilidades.InterseccionHorizontal(listaRectas[i], puntoControl));
                    poligonoUTMRestante.Add(listaRectas[i].punto2);
                }

                else if (listaRectas[i].punto1[1] > puntoControl[1] && listaRectas[i].punto2[1] < puntoControl[1])
	                {
                 rectasCortadas.Add(listaRectas[i]);
                 rectaCorteInferior = i;
                    poligonoUTMGenerado.Add(Utilidades.InterseccionHorizontal(listaRectas[i],puntoControl));
                    poligonoUTMGenerado.Add(listaRectas[i].punto2);
                    poligonoUTMRestante.Add(Utilidades.InterseccionHorizontal(listaRectas[i], puntoControl));
	                }
                    


                // Si los dos puntos estan a la derecha (Y es mayor)
                else if (listaRectas[i].punto1[1] > puntoControl[1] && listaRectas[i].punto2[1] > puntoControl[1])
                {
                    rectasIrrelevantes.Add(listaRectas[i]);
                    poligonoUTMRestante.Add(listaRectas[i].punto2);
                }
               
            }

            

            // Con los dos poligonos ya definidos en UTM, se pasa el generado a LatLng y se calcula su área (en ha)

            for (int i = 0; i < poligonoUTMGenerado.Count; i++)
            {
                PointLatLng control=new PointLatLng();
                control=Utilidades.PasarAWGS(poligonoUTMGenerado[i],utmzone);
                listaPoligonoGenerado.Add(control);
            }
            for (int i = 0; i < poligonoUTMRestante.Count; i++)
            {
                PointLatLng control = new PointLatLng();
                control = Utilidades.PasarAWGS(poligonoUTMRestante[i], utmzone);
                listaPoligonoRestante.Add(control);
            }

            resultado = Utilidades.calcpolygonarea(listaPoligonoGenerado)/10000;
           

            return resultado;
        }

        public static double CalculaAreaVerticalFranja(List<double[]> puntosUTMPoligono, List<Recta> listaRectas, int utmzone, double[] puntoControl, double incrementoMax)
        {
            double resultado;


            iteraciones++;
            List<Recta> rectasCortadas = new List<Recta>();
            List<Recta> rectasIncluidas = new List<Recta>();
            List<Recta> rectasIrrelevantes = new List<Recta>();

            List<double[]> poligonoUTMGenerado = new List<double[]>();
            List<double[]> poligonoUTMRestante = new List<double[]>();

            listaPoligonoGeneradoTemporal.Clear();
            listaPoligonoRestanteTemporal.Clear();


            int rectaCorteSuperior = 99, rectaCorteInferior = 99; // Los identificadores de la posicion de las rectas de corte

            // Si es la primera recta, el punto de control es el primer punto del poligono (siempre a la izquierda)
            if (iteraciones == 1)
            {
                puntoControl[0] = puntosUTMPoligono[0][0];
                puntoControl[1] = puntosUTMPoligono[0][1];
            }


            // Incrementamos la dirección X en un incrementoLineal
            double incremento = IncrementoLineal(incrementoMax);
            puntoControl[0] += incremento;


            // Generamos una recta vertical a una distancia igual al incrementoLineal


            // Evaluamos los puntos de cada recta, donde quedan en relacion a la recta vertical (izquierda o derecha) y añadimos los puntos al poligono generado y al restante
            // Primero el punto que seguro que está a la izquierda, el primero del poligono.
            poligonoUTMGenerado.Add(puntosUTMPoligono[0]);


            for (int i = 0; i < listaRectas.Count; i++)
            {

                // Si los dos puntos estan a la izquierda (Y es menor)
                if (listaRectas[i].punto1[0] < puntoControl[0] && listaRectas[i].punto2[0] < puntoControl[0])
                {
                    rectasIncluidas.Add(listaRectas[i]);
                    poligonoUTMGenerado.Add(listaRectas[i].punto2);
                }
                // Si uno dos puntos esta a la izquierda (Y es menor) y el otro a la derecha 
                else if (listaRectas[i].punto1[0] < puntoControl[0] && listaRectas[i].punto2[0] > puntoControl[0])
                {
                    rectasCortadas.Add(listaRectas[i]);
                    rectaCorteSuperior = i;

                    poligonoUTMGenerado.Add(Utilidades.InterseccionVertical(listaRectas[i], puntoControl));
                    poligonoUTMRestante.Add(Utilidades.InterseccionVertical(listaRectas[i], puntoControl));
                    poligonoUTMRestante.Add(listaRectas[i].punto2);
                }
                // o al revés
                else if (listaRectas[i].punto1[0] > puntoControl[0] && listaRectas[i].punto2[0] < puntoControl[0])
                {
                    rectasCortadas.Add(listaRectas[i]);
                    rectaCorteInferior = i;
                    poligonoUTMGenerado.Add(Utilidades.InterseccionVertical(listaRectas[i], puntoControl));
                    poligonoUTMGenerado.Add(listaRectas[i].punto2);
                    poligonoUTMRestante.Add(Utilidades.InterseccionVertical(listaRectas[i], puntoControl));
                }

                // Si los dos puntos estan a la derecha (Y es mayor)
                else if (listaRectas[i].punto1[0] > puntoControl[0] && listaRectas[i].punto2[0] > puntoControl[0])
                {
                    rectasIrrelevantes.Add(listaRectas[i]);
                    poligonoUTMRestante.Add(listaRectas[i].punto2);
                }

            }



            // Con los dos poligonos ya definidos en UTM, se pasan a LatLng y se calcula su área (en ha)

            for (int i = 0; i < poligonoUTMGenerado.Count; i++)
            {
                PointLatLng control = new PointLatLng();
                control = Utilidades.PasarAWGS(poligonoUTMGenerado[i], utmzone);
                listaPoligonoGeneradoTemporal.Add(control);
            }
            for (int i = 0; i < poligonoUTMRestante.Count; i++)
            {
                PointLatLng control = new PointLatLng();
                control = Utilidades.PasarAWGS(poligonoUTMRestante[i], utmzone);
                listaPoligonoRestanteTemporal.Add(control);
            }

            resultado = Utilidades.calcpolygonarea(listaPoligonoGeneradoTemporal) / 10000;


            return resultado;
        }

        public static double CalculaAreaHorizontalFranja(List<double[]> puntosUTMPoligono, List<Recta> listaRectas, int utmzone, double[] puntoControl, double incrementoMax)
        {
            double resultado;


            iteraciones++;
            List<Recta> rectasCortadas = new List<Recta>();
            List<Recta> rectasIncluidas = new List<Recta>();
            List<Recta> rectasIrrelevantes = new List<Recta>();

            List<double[]> poligonoUTMGenerado = new List<double[]>();
            List<double[]> poligonoUTMRestante = new List<double[]>();

            listaPoligonoGeneradoTemporal.Clear();
            listaPoligonoRestanteTemporal.Clear();


            int rectaCorteSuperior = 99, rectaCorteInferior = 99; // Los identificadores de la posicion de las rectas de corte

            // Si es la primera recta, el punto de control es el primer punto del poligono
            if (iteraciones == 1)
            {
                puntoControl[0] = puntosUTMPoligono[0][0];
                puntoControl[1] = puntosUTMPoligono[0][1];
            }


            // Incrementamos la dirección X en un incrementoLineal
            double incremento = IncrementoLineal(incrementoMax);
            puntoControl[1] += incremento;

            // Evaluamos los puntos de cada recta, donde quedan en relacion a la recta vertical (izquierda o derecha) y añadimos los puntos al poligono generado y al restante
            // Primero el punto que seguro que está a la izquierda, el primero del poligono.
            poligonoUTMGenerado.Add(puntosUTMPoligono[0]);


            for (int i = 0; i < listaRectas.Count; i++)
            {

                // Si los dos puntos estan a la izquierda (Y es menor)
                if (listaRectas[i].punto1[1] < puntoControl[1] && listaRectas[i].punto2[1] < puntoControl[1])
                {
                    rectasIncluidas.Add(listaRectas[i]);
                    poligonoUTMGenerado.Add(listaRectas[i].punto2);
                }
                // Si uno dos puntos esta a la izquierda (Y es menor) y el otro a la derecha o al revés
                else if (listaRectas[i].punto1[1] < puntoControl[1] && listaRectas[i].punto2[1] > puntoControl[1])
                {
                    rectasCortadas.Add(listaRectas[i]);
                    rectaCorteSuperior = i;

                    poligonoUTMGenerado.Add(Utilidades.InterseccionHorizontal(listaRectas[i], puntoControl));
                    poligonoUTMRestante.Add(Utilidades.InterseccionHorizontal(listaRectas[i], puntoControl));
                    poligonoUTMRestante.Add(listaRectas[i].punto2);
                }

                else if (listaRectas[i].punto1[1] > puntoControl[1] && listaRectas[i].punto2[1] < puntoControl[1])
                {
                    rectasCortadas.Add(listaRectas[i]);
                    rectaCorteInferior = i;
                    poligonoUTMGenerado.Add(Utilidades.InterseccionHorizontal(listaRectas[i], puntoControl));
                    poligonoUTMGenerado.Add(listaRectas[i].punto2);
                    poligonoUTMRestante.Add(Utilidades.InterseccionHorizontal(listaRectas[i], puntoControl));
                }



                // Si los dos puntos estan a la derecha (Y es mayor)
                else if (listaRectas[i].punto1[1] > puntoControl[1] && listaRectas[i].punto2[1] > puntoControl[1])
                {
                    rectasIrrelevantes.Add(listaRectas[i]);
                    poligonoUTMRestante.Add(listaRectas[i].punto2);
                }

            }



            // Con los dos poligonos ya definidos en UTM, se pasa el generado a LatLng y se calcula su área (en ha)

            for (int i = 0; i < poligonoUTMGenerado.Count; i++)
            {
                PointLatLng control = new PointLatLng();
                control = Utilidades.PasarAWGS(poligonoUTMGenerado[i], utmzone);
                listaPoligonoGeneradoTemporal.Add(control);
            }
            for (int i = 0; i < poligonoUTMRestante.Count; i++)
            {
                PointLatLng control = new PointLatLng();
                control = Utilidades.PasarAWGS(poligonoUTMRestante[i], utmzone);
                listaPoligonoRestanteTemporal.Add(control);
            }

            resultado = Utilidades.calcpolygonarea(listaPoligonoGeneradoTemporal) / 10000;


            return resultado;
        }

        public static void SacaRectas(List<PointLatLng> puntosWGSPoligono)
        {
            listaRectas.Clear();
            for (int i = 1; i < puntosWGSPoligono.Count; i++)
            {
                PointLatLng P1 = puntosWGSPoligono[i - 1];
                PointLatLng P2 = puntosWGSPoligono[i];
                Recta recta = new Recta(P1, P2);
                listaRectas.Add(recta);
            }
            
        }
        public static void SacaRectas(List<double[]> puntosUTMPoligono,int utmzone)
        {
            listaRectas.Clear();
            for (int i = 1; i < puntosUTMPoligono.Count; i++)
            {
                double[] P1 = puntosUTMPoligono[i - 1];
                double[] P2 = puntosUTMPoligono[i];
                Recta recta = new Recta(P1,P2,utmzone);
                listaRectas.Add(recta);
            }

        }
    }
}
