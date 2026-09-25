using ProyectoWeb.Models;
using System;
using System.Text;

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

            if (libro.ISBN == nodo.Dato.ISBN) return nodo; // Si el ISBN existe, se descarta

            if (libro.ISBN < nodo.Dato.ISBN)
                nodo.Izquierdo = InsertarRec(nodo.Izquierdo, libro);
            else
                nodo.Derecho = InsertarRec(nodo.Derecho, libro);

            nodo.Altura = 1 + Math.Max(ObtenerAltura(nodo.Izquierdo), ObtenerAltura(nodo.Derecho));

            int balance = ObtenerBalance(nodo);

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

        public Libro ObtenerMenor()
        {
            if (Raiz == null) return null;

            NodoLibro actual = Raiz;
            while (actual.Izquierdo != null)
            {
                actual = actual.Izquierdo;
            }
            return actual.Dato;
        }

        public Libro ObtenerMayor()
        {
            if (Raiz == null) return null;

            NodoLibro actual = Raiz;
            while (actual.Derecho != null)
            {
                actual = actual.Derecho;
            }
            return actual.Dato;
        }

        public void Eliminar(long isbn)
        {
            Raiz = EliminarRec(Raiz, isbn);
        }

        private NodoLibro EliminarRec(NodoLibro root, long isbn)
        {
            if (root == null) return root;

            if (isbn < root.Dato.ISBN)
                root.Izquierdo = EliminarRec(root.Izquierdo, isbn);
            else if (isbn > root.Dato.ISBN)
                root.Derecho = EliminarRec(root.Derecho, isbn);
            else
            {
                if ((root.Izquierdo == null) || (root.Derecho == null))
                {
                    NodoLibro temp = root.Izquierdo ?? root.Derecho;

                    if (temp == null)
                    {
                        temp = root;
                        root = null;
                    }
                    else
                        root = temp;
                }
                else
                {
                    NodoLibro temp = ObtenerNodoMenor(root.Derecho);
                    root.Dato = temp.Dato;
                    root.Derecho = EliminarRec(root.Derecho, temp.Dato.ISBN);
                }
            }

            if (root == null) return root;

            root.Altura = Math.Max(ObtenerAltura(root.Izquierdo), ObtenerAltura(root.Derecho)) + 1;

            int balance = ObtenerBalance(root);

            if (balance > 1 && ObtenerBalance(root.Izquierdo) >= 0)
                return RotarDerecha(root);

            if (balance > 1 && ObtenerBalance(root.Izquierdo) < 0)
            {
                root.Izquierdo = RotarIzquierda(root.Izquierdo);
                return RotarDerecha(root);
            }

            if (balance < -1 && ObtenerBalance(root.Derecho) <= 0)
                return RotarIzquierda(root);

            if (balance < -1 && ObtenerBalance(root.Derecho) > 0)
            {
                root.Derecho = RotarDerecha(root.Derecho);
                return RotarIzquierda(root);
            }

            return root;
        }

        private NodoLibro ObtenerNodoMenor(NodoLibro nodo)
        {
            NodoLibro actual = nodo;
            while (actual.Izquierdo != null)
                actual = actual.Izquierdo;
            return actual;
        }

        public string ObtenerLibrosInOrden()
        {
            if (Raiz == null) return "No hay libros en esta categoría.";

            StringBuilder sb = new StringBuilder();
            ObtenerInOrdenRec(Raiz, sb);
            return sb.ToString();
        }

        private void ObtenerInOrdenRec(NodoLibro nodo, StringBuilder sb)
        {
            if (nodo != null)
            {
                ObtenerInOrdenRec(nodo.Izquierdo, sb);
                sb.AppendLine($"ISBN: {nodo.Dato.ISBN} | Título: {nodo.Dato.Titulo} | Autor: {nodo.Dato.Autor}");
                ObtenerInOrdenRec(nodo.Derecho, sb);
            }
        }

        public string GenerarGraphviz()
        {
            if (Raiz == null) return "digraph AVL { node [shape=box]; \"Árbol Vacío\"; }";

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("digraph AVL {");
            sb.AppendLine("    node [shape=record, style=filled, fillcolor=lightblue, fontname=\"Arial\"];");
            
            GenerarNodosGraphviz(Raiz, sb);

            sb.AppendLine("}");
            return sb.ToString();
        }

        private void GenerarNodosGraphviz(NodoLibro nodo, StringBuilder sb)
        {
            if (nodo == null) return;

            sb.AppendLine($"    nodo{nodo.Dato.ISBN} [label=\"<f0> ISBN: {nodo.Dato.ISBN} | <f1> {nodo.Dato.Titulo} | <f2> Altura: {nodo.Altura}\"];");

            if (nodo.Izquierdo != null)
            {
                sb.AppendLine($"    nodo{nodo.Dato.ISBN}:f0 -> nodo{nodo.Izquierdo.Dato.ISBN}:f0;");
                GenerarNodosGraphviz(nodo.Izquierdo, sb);
            }

            if (nodo.Derecho != null)
            {
                sb.AppendLine($"    nodo{nodo.Dato.ISBN}:f0 -> nodo{nodo.Derecho.Dato.ISBN}:f0;");
                GenerarNodosGraphviz(nodo.Derecho, sb);
            }
        }
    }
}