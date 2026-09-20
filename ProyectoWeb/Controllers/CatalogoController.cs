using Microsoft.AspNetCore.Mvc;
using ProyectoWeb.Estructuras;
using ProyectoWeb.Models;

namespace ProyectoWeb.Controllers
{
    public class CatalogoController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            Libro[] listaLibros = ObtenerTodosLosLibros();
            return View(listaLibros);
        }

        [HttpPost]
        public IActionResult Buscar(long isbn)
        {
            Libro libroEncontrado = BuscarEnJerarquia(CargaController.JerarquiaCategorias.Raiz, isbn);
            
            if (libroEncontrado == null)
            {
                ViewBag.Mensaje = $"No se encontró ningún libro con el ISBN: {isbn}";
                return View("Index", new Libro[0]);
            }

            return View("Index", new Libro[] { libroEncontrado });
        }

        private Libro[] ObtenerTodosLosLibros()
        {
            int totalLibros = ContarLibrosEnJerarquia(CargaController.JerarquiaCategorias.Raiz);
            Libro[] arreglo = new Libro[totalLibros];
            int indice = 0;
            
            RecolectarLibros(CargaController.JerarquiaCategorias.Raiz, arreglo, ref indice);
            return arreglo;
        }

        private int ContarLibrosEnJerarquia(NodoCategoria nodoCat)
        {
            if (nodoCat == null) return 0;

            int conteo = ContarNodosAVL(nodoCat.Libros.Raiz);

            NodoCategoria sub = nodoCat.Subcategorias.Cabeza;
            while (sub != null)
            {
                conteo += ContarLibrosEnJerarquia(sub);
                sub = sub.Siguiente;
            }

            return conteo;
        }

        private int ContarNodosAVL(NodoLibro nodo)
        {
            if (nodo == null) return 0;
            return 1 + ContarNodosAVL(nodo.Izquierdo) + ContarNodosAVL(nodo.Derecho);
        }

        private void RecolectarLibros(NodoCategoria nodoCat, Libro[] arreglo, ref int indice)
        {
            if (nodoCat == null) return;

            LlenarArregloInOrden(nodoCat.Libros.Raiz, arreglo, ref indice);

            NodoCategoria sub = nodoCat.Subcategorias.Cabeza;
            while (sub != null)
            {
                RecolectarLibros(sub, arreglo, ref indice);
                sub = sub.Siguiente;
            }
        }

        private void LlenarArregloInOrden(NodoLibro nodo, Libro[] arreglo, ref int indice)
        {
            if (nodo != null)
            {
                LlenarArregloInOrden(nodo.Izquierdo, arreglo, ref indice);
                arreglo[indice++] = nodo.Dato;
                LlenarArregloInOrden(nodo.Derecho, arreglo, ref indice);
            }
        }

        private Libro BuscarEnJerarquia(NodoCategoria nodoCat, long isbn)
        {
            if (nodoCat == null) return null;

            Libro hallado = nodoCat.Libros.Buscar(isbn);
            if (hallado != null) return hallado;

            NodoCategoria sub = nodoCat.Subcategorias.Cabeza;
            while (sub != null)
            {
                hallado = BuscarEnJerarquia(sub, isbn);
                if (hallado != null) return hallado;
                sub = sub.Siguiente;
            }

            return null;
        }
    }
}