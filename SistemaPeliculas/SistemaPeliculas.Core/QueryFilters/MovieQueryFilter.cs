namespace SistemaPeliculas.Core.QueryFilters
{
    // Filtros opcionales del catálogo: GET api/movies?title=padre&genre=Drama&year=1972
    // Si no se manda ninguno, se devuelve todo el catálogo.
    public class MovieQueryFilter
    {
        public string? Title { get; set; }   // contiene este texto (sin importar mayúsculas)
        public string? Genre { get; set; }   // género exacto de la lista válida
        public int? Year { get; set; }       // año de estreno
    }
}
