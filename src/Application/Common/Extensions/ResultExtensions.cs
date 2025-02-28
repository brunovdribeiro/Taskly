using Ardalis.Result;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace Application.Common.Extensions
{
    public static class ResultExtensions
    {
        public static IResult ToProblemResult(
            this Result result, 
            int? statusCode = StatusCodes.Status400BadRequest)
        {
            return Results.Problem(
                detail: result.Errors.FirstOrDefault(),
                statusCode: statusCode
            );
        }

        public static IActionResult ToActionResult<T>(
            this Result<T> result, 
            int? successStatusCode = StatusCodes.Status200OK)
        {
            return result.IsSuccess
                ? new OkObjectResult(result.Value) { StatusCode = successStatusCode }
                : new BadRequestObjectResult(result.Errors);
        }
    }
}