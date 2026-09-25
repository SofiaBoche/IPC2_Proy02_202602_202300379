using ProyectoWeb.Models;
using System;
using System.Text;

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

            Categoria nuevaCat = new Categoria(nombre, padre);
            NodoCategoria nuevoNodo = new NodoCategoria(nuevaCat);

            if (!string.IsNullOrEmpty(padre))
            {
                NodoCategoria nodoPadre = BuscarRecursivo(Raiz, padre);
                if (nodoPadre != null)
                {
                    InsertarOrdenado(nodoPadre, nuevoNodo);
                    return nuevoNodo;
                }
            }

            InsertarOrdenado(Raiz, nuevoNodo);
            return nuevoNodo;
        }

        private void InsertarOrdenado(NodoCategoria padre, NodoCategoria nuevo)
        {
            if (padre.Subcategorias.Cabeza == null)
            {
                padre.Subcategorias.InsertarOrdenado(nuevo);
                return;
            }

            if (string.Compare(nuevo.Dato.Nombre, padre.Subcategorias.Cabeza.Dato.Nombre, StringComparison.OrdinalIgnoreCase) < 0)
            {
                nuevo.Siguiente = padre.Subcategorias.Cabeza;
                padre.Subcategorias.InsertarOrdenado(nuevo);
                return;
            }

            NodoCategoria actual = padre.Subcategorias.Cabeza;
            while (actual.Siguiente != null && 
                   string.Compare(actual.Siguiente.Dato.Nombre, nuevo.Dato.Nombre, StringComparison.OrdinalIgnoreCase) < 0)
            {
                actual = actual.Siguiente;
            }

            nuevo.Siguiente = actual.Siguiente;
            actual.Siguiente = nuevo;
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


        public string MostrarEstructura()
        {
            if (Raiz == null) return "Catálogo vacío.";

            StringBuilder sb = new StringBuilder();
            MostrarEstructuraRec(Raiz, sb, 0);
            return sb.ToString();
        }

        private void MostrarEstructuraRec(NodoCategoria nodo, StringBuilder sb, int nivel)
        {
            if (nodo == null) return;

            string indentacion = new string('-', nivel * 4);
            sb.AppendLine($"{indentacion}> {nodo.Dato.Nombre}");

            NodoCategoria sub = nodo.Subcategorias.Cabeza;
            while (sub != null)
            {
                MostrarEstructuraRec(sub, sb, nivel + 1);
                sub = sub.Siguiente;
            }
        }

        public string GenerarGraphviz(string nombreCategoriaInicio = null)
        {
            NodoCategoria inicio = Raiz;
            if (!string.IsNullOrEmpty(nombreCategoriaInicio))
            {
                inicio = Buscar(nombreCategoriaInicio);
            }

            if (inicio == null) return "digraph ArbolCategorias { node [shape=box]; \"No encontrada\"; }";

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("digraph ArbolCategorias {");
            sb.AppendLine("    node [shape=ellipse, style=filled, fillcolor=lightyellow, fontname=\"Arial\"];");

            GenerarNodosGraphviz(inicio, sb);

            sb.AppendLine("}");
            return sb.ToString();
        }

        private void GenerarNodosGraphviz(NodoCategoria nodo, StringBuilder sb)
        {
            if (nodo == null) return;

            NodoCategoria sub = nodo.Subcategorias.Cabeza;
            while (sub != null)
            {
                sb.AppendLine($"    \"{nodo.Dato.Nombre}\" -> \"{sub.Dato.Nombre}\";");
                GenerarNodosGraphviz(sub, sb);
                sub = sub.Siguiente;
            }
        }
    }
}