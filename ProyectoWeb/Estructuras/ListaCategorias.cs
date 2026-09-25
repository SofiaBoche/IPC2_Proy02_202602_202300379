using ProyectoWeb.Models;

namespace ProyectoWeb.Estructuras
{
    public class ListaCategorias
    {
        public NodoCategoria Cabeza { get; private set; }

        public void AgregarNodo(NodoCategoria nuevo)
        {
            if (Cabeza == null)
            {
                Cabeza = nuevo;
                return;
            }

            NodoCategoria actual = Cabeza;
            while (actual.Siguiente != null)
            {
                actual = actual.Siguiente;
            }
            actual.Siguiente = nuevo;
        }

        public void Agregar(Categoria categoria)
        {
            AgregarNodo(new NodoCategoria(categoria));
        }
    }
}