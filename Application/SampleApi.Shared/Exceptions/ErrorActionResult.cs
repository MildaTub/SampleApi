using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace SampleApi.Shared.Exceptions;

public class ErrorActionResult
{
    public static ObjectResult BadRequest(string errorMessage)
    {
        Error error = ErrorResponse.BadRequest(errorMessage);

        return new ObjectResult(error)
        {
            StatusCode = (int)HttpStatusCode.BadRequest
        };
    }

    public static ObjectResult NotFound(string target)
    {
        Error error = ErrorResponse.NotFound($"{target} not found.");

        return new ObjectResult(error)
        {
            StatusCode = (int)HttpStatusCode.NotFound
        };
    }

    public static ObjectResult InternalServerError()
    {
        Error error = ErrorResponse.InternalServerError();

        return new ObjectResult(error)
        {
            StatusCode = (int)HttpStatusCode.InternalServerError
        };
    }
}