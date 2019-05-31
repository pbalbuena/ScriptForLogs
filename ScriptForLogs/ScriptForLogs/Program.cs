using OfficeOpenXml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ScriptForLogs
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {

                string linea;
                String[] datos = null;
                ArrayList direcciones = new ArrayList();
                String directory = Environment.CurrentDirectory;
                //Sacar nombres e emails del .txt-----------------------------------------
                //p.DataFile = new StreamReader(directory + "/datos.txt");
                StreamReader DataFile = new StreamReader(directory + "/datos.txt");
                //parsear el documento de datos
                System.IO.StreamReader file2 = new System.IO.StreamReader(directory + "/repositorios.txt");
                while ((linea = file2.ReadLine()) != null)
                {
                    direcciones.Add(linea);
                }
                int n = 0;
                foreach (var d in direcciones)
                {
                    Console.WriteLine("PATH: " + d.ToString());
                    System.IO.StreamReader file = new System.IO.StreamReader(directory + "/datos.txt");
                    while ((linea = file.ReadLine()) != null)
                    {

                        datos = linea.Split(',');
                        String userMapfre = datos[0];
                        String userCap = datos[1];
                        String emailMapfre = datos[2];
                        String emailCap = datos[3];
                        string fechaFin = DateTime.Now.ToString("yyyy-MM-dd");
                        string fechaInicio = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
                        //si se añade el campo opcional de fecha
                        if (datos.Length == 6)
                        {
                            fechaInicio = datos[4];
                            fechaFin = datos[5];
                        }

                        /*
                         * to name log from user Mapfre
                         * 
                        String[] simpleUserFormat = null;
                        String simpleUser = "";
                        simpleUserFormat = userMapfre.Split(' ');
                        foreach(String word in simpleUserFormat)
                        {
                            simpleUser+=word + "_";
                        }
                        */

                        String logName = "log_" + userCap + "_" + fechaInicio + "_" + fechaFin;

                        String repoName = d.ToString().Split('\\').Last();
                        String fileNameForLog = getData(userMapfre, fechaInicio, fechaFin, repoName +"-"+ logName, d.ToString());

                        editFile(fileNameForLog, userCap, userMapfre, emailCap, emailMapfre);
                    }
                    n++;
                    file.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Console.ReadLine();
            }
            Console.WriteLine("Finished. Press Enter to exit.");
            Console.ReadLine();
        }

        public static void editFile(String fileName, String userCap, String userMapfre,
           String emailCap, String emailMapfre)
        {
            String directory = Environment.CurrentDirectory;
            StreamReader reader = new StreamReader(directory + "/" + fileName + "R.html");
            StreamWriter writer = new StreamWriter(directory + "/" + fileName + ".html", true);

            

            while (reader.EndOfStream == false)
            {
                string line = reader.ReadLine();

                //reemplazamos email y usuario
                line = line.Replace(userMapfre, userCap);
                line = line.Replace(emailMapfre, emailCap);
                line = Regex.Replace(line, "mapfre", "cliente", RegexOptions.IgnoreCase);
                line = Regex.Replace(line, "insureandgo", "cliente", RegexOptions.IgnoreCase);

                //coloreamos las lineas
                if (line.StartsWith("+"))
                {
                    line = "<p style='color:green'>" + line + "</p>";
                }
                if (line.StartsWith("-"))
                {
                    line = "<p style='color:red'>" + line + "</p>";
                }
                writer.Write(line);
            }
            reader.Close();
            writer.Close();
            File.Delete(directory + "/" + fileName + "R.html");
        }

        public static String getData(string userMapfre, string fechaInicio, 
            string fechaFin, string fileName, String directory)
        {
            //comando para descargar el log
            String cmdText = "/C git -C " + directory + " log -p --since=" + fechaInicio + " --until=" + fechaFin + " " +
                "--shortstat --no-merges --author=" + userMapfre + " > " + fileName + "R.html";
            String command = cmdText;
            //abrimos nueva ventana cmd
            ProcessStartInfo cmdsi = new ProcessStartInfo("cmd.exe");
            //le añadimos el comando
            cmdsi.Arguments = command;
            //la ocultamos para que no aparezca
            cmdsi.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            //ejecutamos el comando
            Process cmd = Process.Start(cmdsi);
            //esperamos a que termine la descarga para que al empezar a editar no colisione
            cmd.WaitForExit();
            return fileName;
        }
    }
}
