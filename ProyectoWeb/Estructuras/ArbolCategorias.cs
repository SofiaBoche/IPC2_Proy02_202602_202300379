using ProyectoWeb.Models;
using System;

namespace ProyectoWeb.Estructuras
{
    public class ArbolCategorias
    {
        public NodoCategoria Raiz { get; private set; }

        public NodoCategoria InsertarOCategorizar(string nombre, string padre = null)
        {
            if (Raiz == null)
            {
                Raiz = new NodoCategoria(new Categoria(nombre, padre));
                return Raiz;
            }

            NodoCategoria existente = BuscarRecursivo(Raiz, nombre);
            if (existente != null) return existente;

            if (!string.IsNullOrEmpty(padre))
            {
                NodoCategoria nodoPadre = BuscarRecursivo(Raiz, padre);
                if (nodoPadre != null)
                {
                    Categoria nuevaCat = new Categoria(nombre, padre);
                    NodoCategoria nuevoNodo = new NodoCategoria(nuevaCat);
                    nodoPadre.Subcategorias.AgregarNodo(nuevoNodo);
                    return nuevoNodo;
                }
            }

            Categoria catRaizSecundario = new Categoria(nombre, padre);
            NodoCategoria nodoSec = new NodoCategoria(catRaizSecundario);
            Raiz.Subcategorias.AgregarNodo(nodoSec);
            return nodoSec;
        }

        public NodoCategoria Buscar(string nombre) => BuscarRecursivo(Raiz, nombre);

        private NodoCategoria BuscarRecursivo(NodoCategoria actual, string nombre)
        {
            if (actual == null) return null;
            if (actual.Dato.Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase)) return actual;

            NodoCategoria temp = actual.Subcategorias.Cabeza;
            while (temp != null)
            {
                NodoCategoria hallado = BuscarRecursivo(temp, nombre);
                if (hallado != null) return hallado;
                temp = temp.Siguiente;
            }

            return null;
        }
    }
}