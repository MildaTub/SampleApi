using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SampleApi.Models.Exceptions;
using SampleApi.Shared.Exceptions;

namespace SampleApi.Shared.ExceptionHandlers;

public class MissingEntityExceptionHandler : IExceptionHandler
{
    public bool CanHandle(Type type)
    {
        return type == typeof(MissingEntityException);
    }

    public ObjectResult Handle(Exception ex, HttpContext httpContext)
    {
        MissingEntityException parsedException = (MissingEntityException)Convert.ChangeType(ex, typeof(MissingEntityException));
        return ErrorActionResult.NotFound(parsedException.EntityName);
    }
}