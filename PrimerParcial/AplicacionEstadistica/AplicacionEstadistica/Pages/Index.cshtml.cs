using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Data;

namespace AplicacionEstadistica.Pages
{
    public class IndexModel : PageModel
    {
        // Esta propiedad debe estar definida para que se pase a la vista
        public List<Alumno> Alumnos { get; set; }

        public void OnGet()
        {
            // Crear el DataSet
            DataSet dst = new DataSet();
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "App_Data", "archivo.xml");

            try
            {
                // Leer el esquema XML
                dst.ReadXmlSchema(filePath);
                dst.ReadXml(filePath);

                // Verificar si hay datos
                if (dst.Tables.Count > 0 && dst.Tables[0].Rows.Count > 0)
                {
                    DataTable tbl = dst.Tables[0];
                    Alumnos = new List<Alumno>();

                    foreach (DataRow row in tbl.Rows)
                    {
                        // Obtener calificaciones
                        int parcial1 = Convert.ToInt32(row["Parcial1"]);
                        int parcial2 = Convert.ToInt32(row["Parcial2"]);
                        int parcial3 = Convert.ToInt32(row["Parcial3"]);
                        
                        // Calcular promedio
                        double promedio = (parcial1 + parcial2 + parcial3) / 3.0;

                        // Obtener examen extra
                        int extra = row["Extra"] is DBNull || row["Extra"].ToString() == "NP" ? 0 : Convert.ToInt32(row["Extra"]);

                        // Calcular calificación final
                        double calificacionFinal = Math.Max(promedio, extra);
                        int final = (int)Math.Round(calificacionFinal);

                        // Añadir a la lista de alumnos
                        Alumnos.Add(new Alumno
                        {
                            Id = Convert.ToInt32(row["Id"]),
                            Nombre = row["Nombre"].ToString(),
                            Apellidos = row["Apellidos"].ToString(),
                            Parcial1 = parcial1,
                            Parcial2 = parcial2,
                            Parcial3 = parcial3,
                            Extra = extra,
                            Promedio = (int)promedio,
                            Final = final
                        });
                    }
                }
                else
                {
                    ViewData["Error"] = "No se encontraron datos en el archivo XML.";
                }
            }
            catch (Exception ex)
            {
                ViewData["Error"] = $"Error al leer el archivo XML: {ex.Message}";
            }
        }
    }

    public class Alumno
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public int Parcial1 { get; set; }
        public int Parcial2 { get; set; }
        public int Parcial3 { get; set; }
        public int Extra { get; set; }
        public int Promedio { get; set; }
        public int Final { get; set; }
    }
}
