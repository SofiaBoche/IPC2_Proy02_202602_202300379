using Microsoft.AspNetCore.Mvc;
using ProyectoWeb.Estructuras;
using ProyectoWeb.Models;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ProyectoWeb.Controllers
{
    public class CargaController : Controller
    {
        public static ArbolCategorias JerarquiaCategorias = new ArbolCategorias();
        public static ArbolAVL LibrosGlobales = new ArbolAVL();

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SubirArchivo(IFormFile archivo)
        {
            if (archivo == null || archivo.Length == 0)
            {
                ViewBag.Error = "Por favor, selecciona un archivo XML válido.";
                return View("Index");
            }

            try
            {
                using (var stream = archivo.OpenReadStream())
                {
                    XDocument doc = await XDocument.LoadAsync(stream, LoadOptions.None, CancellationToken.None);

                    var categoriasXml = doc.Descendants("categoria");
                    foreach (var catElem in categoriasXml)
                    {
                        string nombreCat = catElem.Value?.Trim();
                        string padreCat = catElem.Attribute("padre")?.Value?.Trim();

                        if (!string.IsNullOrEmpty(nombreCat))
                        {
                            JerarquiaCategorias.InsertarOCategorizar(nombreCat, padreCat);
                        }
                    }

                    var librosXml = doc.Descendants("libro");
                    foreach (var libroElem in librosXml)
                    {
                        var elemIsbn = libroElem.Element("ISBN")?.Value;
                        var elemTitulo = libroElem.Element("titulo")?.Value;
                        var elemAutor = libroElem.Element("autor")?.Value;
                        var elemCategoria = libroElem.Element("categoria")?.Value;

                        if (!string.IsNullOrEmpty(elemIsbn) && long.TryParse(elemIsbn, out long isbn))
                        {
                            string titulo = elemTitulo ?? "Sin Título";
                            string autor = elemAutor ?? "Desconocido";
                            string nombreCategoria = elemCategoria ?? "Sin Categoria";

                            Libro libro = new Libro(isbn, titulo, autor, nombreCategoria);

                            LibrosGlobales.Insertar(libro);

                            NodoCategoria nodoCat = JerarquiaCategorias.Buscar(nombreCategoria);
                            if (nodoCat == null)
                            {
                                nodoCat = JerarquiaCategorias.InsertarOCategorizar(nombreCategoria);
                            }

                            nodoCat.Libros.Insertar(libro);
                        }
                    }

                    ViewBag.Exito = "¡Archivo XML procesado correctamente de forma incremental!";
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error al procesar el archivo XML: " + ex.Message;
            }

            return View("Index");
        }
    }
}