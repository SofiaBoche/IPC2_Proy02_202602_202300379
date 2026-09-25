using ProyectoWeb.Models;

namespace ProyectoWeb.Estructuras
{
    public class NodoCategoria
    {
        public Categoria Dato { get; set; }
        public ListaCategorias Subcategorias { get; set; } 
        public ArbolAVL Libros { get; set; }               
        public NodoCategoria Siguiente { get; set; }       

        public NodoCategoria(Categoria categoria)
        {
            Dato = categoria;
            Subcategorias = new ListaCategorias();
            Libros = new ArbolAVL();
            Siguiente = null;
        }
    }
}