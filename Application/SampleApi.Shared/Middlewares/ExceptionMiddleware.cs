using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;
using SampleApi.Shared.ExceptionHandlers;
using SampleApi.Shared.Exceptions;

namespace SampleApi.Shared.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    private readonly IActionResultExecutor<ObjectResult> _executor;
    private readonly IEnumerable<IExceptionHandler> _exceptionHandlers;

    public ExceptionMiddleware(RequestDelegate next, IActionResultExecutor<ObjectResult> executor, IEnumerable<IExceptionHandler> exceptionHandlers)
    {
        _executor = executor;
        _next = next;
        _exceptionHandlers = exceptionHandlers;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            if (ex is AggregateException)
            {
                ex = ex.InnerException;
            }

            ObjectResult result = CreateExceptionResult(ex, httpContext);
            await ExecuteAsync(httpContext, result);
        }
    }

    private ObjectResult CreateExceptionResult(Exception ex, HttpContext httpContext)
    {
        IExceptionHandler handler = _exceptionHandlers.FirstOrDefault(exceptionHandler => exceptionHandler.CanHandle(ex.GetType()));

        ObjectResult result = handler != null ? handler.Handle(ex, httpContext) : ErrorActionResult.InternalServerError();
        return result;
    }

    private async Task ExecuteAsync(HttpContext context, ObjectResult error)
    {
        RouteData routeData = context.GetRouteData() ?? new RouteData();
        ActionContext actionContext = new ActionContext(context, routeData, new ActionDescriptor());

        await _executor.ExecuteAsync(actionContext, error);
    }
}