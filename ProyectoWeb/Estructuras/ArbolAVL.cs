using ProyectoWeb.Models;
using System;

namespace ProyectoWeb.Estructuras
{
    public class ArbolAVL
    {
        public NodoLibro Raiz { get; private set; }

        private int ObtenerAltura(NodoLibro n) => n == null ? 0 : n.Altura;

        private int ObtenerBalance(NodoLibro n) => n == null ? 0 : ObtenerAltura(n.Izquierdo) - ObtenerAltura(n.Derecho);

        private NodoLibro RotarDerecha(NodoLibro y)
        {
            NodoLibro x = y.Izquierdo;
            NodoLibro T2 = x.Derecho;

            x.Derecho = y;
            y.Izquierdo = T2;

            y.Altura = Math.Max(ObtenerAltura(y.Izquierdo), ObtenerAltura(y.Derecho)) + 1;
            x.Altura = Math.Max(ObtenerAltura(x.Izquierdo), ObtenerAltura(x.Derecho)) + 1;

            return x;
        }

        private NodoLibro RotarIzquierda(NodoLibro x)
        {
            NodoLibro y = x.Derecho;
            NodoLibro T2 = y.Izquierdo;

            y.Izquierdo = x;
            x.Derecho = T2;

            x.Altura = Math.Max(ObtenerAltura(x.Izquierdo), ObtenerAltura(x.Derecho)) + 1;
            y.Altura = Math.Max(ObtenerAltura(y.Izquierdo), ObtenerAltura(y.Derecho)) + 1;

            return y;
        }

        public void Insertar(Libro libro)
        {
            Raiz = InsertarRec(Raiz, libro);
        }

        private NodoLibro InsertarRec(NodoLibro nodo, Libro libro)
        {
            if (nodo == null) return new NodoLibro(libro);

            // Regla del Auxiliar: Si el ISBN ya existe, se descarta (no se inserta nada nuevo)
            if (libro.ISBN == nodo.Dato.ISBN) return nodo;

            if (libro.ISBN < nodo.Dato.ISBN)
                nodo.Izquierdo = InsertarRec(nodo.Izquierdo, libro);
            else
                nodo.Derecho = InsertarRec(nodo.Derecho, libro);

            nodo.Altura = 1 + Math.Max(ObtenerAltura(nodo.Izquierdo), ObtenerAltura(nodo.Derecho));

            int balance = ObtenerBalance(nodo);

            // Casos de rotación AVL
            if (balance > 1 && libro.ISBN < nodo.Izquierdo.Dato.ISBN)
                return RotarDerecha(nodo);

            if (balance < -1 && libro.ISBN > nodo.Derecho.Dato.ISBN)
                return RotarIzquierda(nodo);

            if (balance > 1 && libro.ISBN > nodo.Izquierdo.Dato.ISBN)
            {
                nodo.Izquierdo = RotarIzquierda(nodo.Izquierdo);
                return RotarDerecha(nodo);
            }

            if (balance < -1 && libro.ISBN < nodo.Derecho.Dato.ISBN)
            {
                nodo.Derecho = RotarDerecha(nodo.Derecho);
                return RotarIzquierda(nodo);
            }

            return nodo;
        }

        public Libro Buscar(long isbn)
        {
            return BuscarRec(Raiz, isbn);
        }

        private Libro BuscarRec(NodoLibro raiz, long isbn)
        {
            if (raiz == null) return null;
            if (raiz.Dato.ISBN == isbn) return raiz.Dato;

            return isbn < raiz.Dato.ISBN 
                ? BuscarRec(raiz.Izquierdo, isbn) 
                : BuscarRec(raiz.Derecho, isbn);
        }
    }
}