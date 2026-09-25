using ProyectoWeb.Models;

namespace ProyectoWeb.Estructuras
{
    public class NodoLibro
    {
        public Libro Dato { get; set; }
        public NodoLibro Izquierdo { get; set; }
        public NodoLibro Derecho { get; set; }
        public int Altura { get; set; }

        public NodoLibro(Libro libro)
        {
            Dato = libro;
            Altura = 1;
        }
    }
}