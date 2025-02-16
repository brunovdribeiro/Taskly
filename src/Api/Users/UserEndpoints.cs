using Application.Common.Exceptions;
using Application.Features.Users.Commands.CreateUser;
using Application.Features.Users.Commands.DeactivateUser;
using Application.Features.Users.Dtos;
using Application.Features.Users.Interfaces;
using Application.Features.Users.Queries.GetUserByIdQuery;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

public static class UserEndpoints
{
    public static RouteGroupBuilder MapUserEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/users");

        group.MapGet("/{id}", GetUserById);
        group.MapPost("/", CreateUser);
        group.MapPut("/{id}/deactivate", DeactivateUser); // Add this line


        return group;
    }

    private static async Task<Results<Ok<UserDto>, NotFound>> GetUserById(
        Guid id,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var query = new GetUserByIdQuery(id);
        var user = await mediator.Send(query, cancellationToken);

        if (user is null)
            return TypedResults.NotFound();

        return TypedResults.Ok(user);
    }

    private static async Task<Results<Created<UserDto>, ValidationProblem>> CreateUser(
        CreateUserDto createUserDto,
        IMediator mediator,
        IUserRead userRead,
        CancellationToken cancellationToken)
    {
        var command = new CreateUserCommand
        {
            Email = createUserDto.Email,
            Name = createUserDto.Name
        };

        var user = await mediator.Send(command, cancellationToken);

        return TypedResults.Created($"/api/users/{user}", user);
    }
    
    private static async Task<Results<Ok<UserDto>, NotFound>> DeactivateUser(
        Guid id,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var command = new DeactivateUserCommand { UserId = id };
        
        try
        {
            var user = await mediator.Send(command, cancellationToken);
            return TypedResults.Ok(user);
        }
        catch (NotFoundException)
        {
            return TypedResults.NotFound();
        }
    }
}