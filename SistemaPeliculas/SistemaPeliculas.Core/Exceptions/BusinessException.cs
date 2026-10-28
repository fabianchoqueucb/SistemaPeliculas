namespace SistemaPeliculas.Core.Exceptions
{
    // Error de regla de negocio. StatusCode: 400 por defecto, 404 cuando algo no existe.
    public class BusinessException : Exception
    {
        public int StatusCode { get; }

        public BusinessException(string message, int statusCode = 400) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
