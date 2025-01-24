using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SampleApi.Models.Exceptions;
using SampleApi.Shared.Exceptions;

namespace SampleApi.Shared.ExceptionHandlers;

public sealed class DomainExceptionHandler : IExceptionHandler
{
    public bool CanHandle(Type type)
    {
        return type == typeof(DomainException);
    }

    public ObjectResult Handle(Exception ex, HttpContext httpContext)
    {
        return ErrorActionResult.BadRequest(ex.Message);
    }
}