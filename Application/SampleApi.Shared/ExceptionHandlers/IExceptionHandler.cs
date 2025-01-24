using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SampleApi.Shared.ExceptionHandlers;

public interface IExceptionHandler
{
    bool CanHandle(Type type);
    ObjectResult Handle(Exception ex, HttpContext httpContext);
}