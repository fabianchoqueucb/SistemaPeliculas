using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SistemaPeliculas.Core.Exceptions;

namespace SistemaPeliculas.Api.Filters
{
    // Convierte las BusinessException en respuestas HTTP con un mensaje claro
    public class GlobalExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is BusinessException ex)
            {
                context.Result = new ObjectResult(new
                {
                    status = ex.StatusCode,
                    title = ex.StatusCode == 404 ? "No encontrado" : "Regla de negocio",
                    detail = ex.Message
                })
                {
                    StatusCode = ex.StatusCode
                };
                context.ExceptionHandled = true;
            }
        }
    }
}
