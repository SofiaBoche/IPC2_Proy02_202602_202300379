using Microsoft.AspNetCore.Mvc;
using ProyectoWeb.Estructuras;
using ProyectoWeb.Models;
using System.Xml.Linq;

namespace ProyectoWeb.Controllers
{
    public class CargaController : Controller
    {
        public static ArbolCategorias JerarquiaCategorias = new ArbolCategorias();

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
                ViewBag.Error = "Por favor, selecciona un archivo válido.";
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
                        string nombreCat = catElem.Value.Trim();
                        string padreCat = catElem.Attribute("padre")?.Value?.Trim();

                        if (!string.IsNullOrEmpty(nombreCat))
                        {
                            JerarquiaCategorias.InsertarOCategorizar(nombreCat, padreCat);
                        }
                    }

                    var librosXml = doc.Descendants("libro");
                    foreach (var libroElem in librosXml)
                    {
                        long isbn = Convert.ToInt64(libroElem.Element("ISBN")?.Value);
                        string titulo = libroElem.Element("titulo")?.Value;
                        string autor = libroElem.Element("autor")?.Value;
                        string nombreCategoria = libroElem.Element("categoria")?.Value;

                        Libro libro = new Libro(isbn, titulo, autor, nombreCategoria);

                        NodoCategoria nodoCat = JerarquiaCategorias.Buscar(nombreCategoria);
                        if (nodoCat == null)
                        {
                            nodoCat = JerarquiaCategorias.InsertarOCategorizar(nombreCategoria);
                        }

                        nodoCat.Libros.Insertar(libro);
                    }

                    ViewBag.Exito = "¡Archivo XML procesado exitosamente en la jerarquía de categorías y árboles AVL!";
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