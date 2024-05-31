using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Helpers;

public class ApiOperationResult<TResult> : ApiOperationResult
{
    public static ApiOperationResult<TResult> Success(TResult result) => new(result);

    public static ApiOperationResult<TResult> Failure(Error error) => new(error);

    public static ApiOperationResult<TResult> Failure(HttpStatusCode statusCode, string message) =>
        new(new Error(statusCode, message));

    private ApiOperationResult(TResult result) => Result = result;

    private ApiOperationResult(Error error) : base(error)
    {
    }

    public TResult Result { get; }

    public static implicit operator ApiOperationResult<TResult>(Error error) => Failure(error);

    public static implicit operator ApiOperationResult<TResult>(TResult result) => Success(result);

    public new ActionResult<TResult> ToActionResult(HttpStatusCode statusCode = HttpStatusCode.OK) =>
        IsSuccess
            ? new ObjectResult(Result) { StatusCode = (int)statusCode }
            : new ObjectResult(Error.ToErrorResponse()) { StatusCode = (int)Error.StatusCode };
}