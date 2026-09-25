using ProyectoWeb.Models;
using System;

namespace ProyectoWeb.Estructuras
{
    public class ListaCategorias
    {
        public NodoCategoria Cabeza { get; private set; }

        public void InsertarOrdenado(NodoCategoria nuevo)
        {
            if (Cabeza == null)
            {
                Cabeza = nuevo;
                return;
            }

            if (string.Compare(nuevo.Dato.Nombre, Cabeza.Dato.Nombre, StringComparison.OrdinalIgnoreCase) < 0)
            {
                nuevo.Siguiente = Cabeza;
                Cabeza = nuevo;
                return;
            }

            NodoCategoria actual = Cabeza;
            while (actual.Siguiente != null && 
                   string.Compare(actual.Siguiente.Dato.Nombre, nuevo.Dato.Nombre, StringComparison.OrdinalIgnoreCase) < 0)
            {
                actual = actual.Siguiente;
            }

            nuevo.Siguiente = actual.Siguiente;
            actual.Siguiente = nuevo;
        }

        public void AgregarNodo(NodoCategoria nuevo)
        {
            InsertarOrdenado(nuevo);
        }

        public void Agregar(Categoria categoria)
        {
            InsertarOrdenado(new NodoCategoria(categoria));
        }
    }
}