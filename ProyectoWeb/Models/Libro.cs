namespace ProyectoWeb.Models
{
    public class Libro
    {
        public long ISBN { get; set; }
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public string Categoria { get; set; }

        public Libro(long isbn, string titulo, string autor, string categoria)
        {
            ISBN = isbn;
            Titulo = titulo;
            Autor = autor;
            Categoria = categoria;
        }
    }
}