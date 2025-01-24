namespace SampleApi.Shared.Exceptions;

public static class ErrorResponse
{
    public static Error BadRequest(string errorMessage)
    {
        return new Error(ErrorCode.BadRequest, errorMessage);
    }

    public static Error NotFound(string errorMessage)
    {
        return new Error(ErrorCode.NotFound, errorMessage);
    }

    public static Error InternalServerError()
    {
        return InternalServerError("Internal server error.");
    }

    public static Error InternalServerError(string errorMessage)
    {
        return new Error(ErrorCode.InternalServerError, errorMessage);
    }
}