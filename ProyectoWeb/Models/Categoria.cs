namespace ProyectoWeb.Models
{
    public class Categoria
    {
        public string Nombre { get; set; }
        public string Padre { get; set; }

        public Categoria(string nombre, string padre = null)
        {
            Nombre = nombre;
            Padre = padre;
        }
    }
}
