using Application.Features.Users.Dtos;
using Application.Features.Users.Queries.GetUserByIdQuery;
using Ardalis.Result;
using FastEndpoints;
using MediatR;

namespace Api.Users.v1.GetUserById;

public class GetUserByIdEndpoint(
    IMediator mediator
) : Endpoint<GetUserByIdRequest, UserDto>
{
    public override void Configure()
    {
        Get("/users/{Id}");
        Version(1);
        AllowAnonymous();
        Options(x => 
        {
            x.WithTags("Users")
                .WithSummary("Get user details by ID")
                .WithDescription("Retrieves detailed information about a specific user")
                .WithName("GetUserById")
                .Produces<UserDto>()
                .Produces(StatusCodes.Status404NotFound);
        });
    }

    public override async Task HandleAsync(GetUserByIdRequest req, CancellationToken ct)
    {
        var query = new GetUserByIdQuery(req.Id);
        var result = await mediator.Send(query, ct);

        if (result.IsNotFound())
        {
            await SendNotFoundAsync(ct);
            return;
        }

        Response = result.Value;
    }
}