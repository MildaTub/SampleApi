using System.ComponentModel.DataAnnotations;

namespace SampleApi.Shared.Exceptions;

public class Error
{
    public string Code { get; set; }
    public string Message { get; set; }

    public Error(ErrorCode code, string message)
    {
        Code = code.ToString();
        Message = message;
    }
}