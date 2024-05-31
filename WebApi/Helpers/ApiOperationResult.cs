using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Helpers;

public class ApiOperationResult
{
    public static ApiOperationResult Success() => new();

    public static ApiOperationResult Failure(Error error) => new(error);

    public static ApiOperationResult Failure(HttpStatusCode statusCode, string message) =>
        new(new Error(statusCode, message));

    public static ApiOperationResult<TResult> Success<TResult>(TResult result) =>
        ApiOperationResult<TResult>.Success(result);

    protected ApiOperationResult() => IsSuccess = true;

    protected ApiOperationResult(Error error)
    {
        IsSuccess = false;
        Error = error;
    }

    public bool IsSuccess { get; }

    public Error Error { get; }

    public static implicit operator ApiOperationResult(Error error) => Failure(error);
    
    public ActionResult ToActionResult(HttpStatusCode successCode = HttpStatusCode.OK) =>
        IsSuccess
            ? new StatusCodeResult((int)successCode)
            : new ObjectResult(Error.ToErrorResponse()) { StatusCode = (int)Error.StatusCode };
}