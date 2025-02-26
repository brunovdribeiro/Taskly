using Application.Common.Exceptions;
using Application.Features.Users.Commands.Activate;
using Application.Features.Users.Commands.CreateUser;
using Application.Features.Users.Commands.DeactivateUser;
using Application.Features.Users.Dtos;
using Application.Features.Users.Interfaces;
using Application.Features.Users.Queries.GetAllUsers;
using Application.Features.Users.Queries.GetUserByIdQuery;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

public static class UserEndpoints
{
    public static IEndpointRouteBuilder MapUserEndpoints(
        this IEndpointRouteBuilder routes
    )
    {
        var group = routes.MapGroup("/users")
            .WithTags("Users")
            .WithOpenApi();

        group.MapGet("/{id}", GetUserById)
            .WithName("GetUserById")
            .WithSummary("Get a user by ID")
            .WithDescription("Retrieves a specific user by their unique identifier")
            .WithOpenApi(operation =>
            {
                operation.Parameters[0].Description = "The unique identifier of the user";
                operation.Responses["200"].Description = "User found successfully";
                operation.Responses["404"].Description = "User not found";
                return operation;
            })
            .Produces<UserDto>(200)
            .Produces(404);

        group.MapPost("/", CreateUser)
            .WithName("CreateUser")
            .WithSummary("Create a new user")
            .WithDescription("Creates a new user with the provided information")
            .WithOpenApi(operation =>
            {
                operation.RequestBody.Description = "User creation data";
                operation.Responses["201"].Description = "User created successfully";
                operation.Responses["400"].Description = "Invalid request data";
                return operation;
            })
            .Produces<UserDto>(201)
            .ProducesValidationProblem();

        group.MapPut("/{id}/deactivate", DeactivateUser)
            .WithName("DeactivateUser")
            .WithSummary("Deactivate a user")
            .WithDescription("Deactivates an existing user by their ID")
            .WithOpenApi(operation =>
            {
                operation.Parameters[0].Description = "The unique identifier of the user to deactivate";
                operation.Responses["200"].Description = "User deactivated successfully";
                operation.Responses["404"].Description = "User not found";
                return operation;
            })
            .Produces<UserDto>(200)
            .Produces(404);

        group.MapPut("/{id}/activate", ActivateUser)
            .WithName("ActivateUser")
            .WithSummary("Activate a user")
            .WithDescription("Activates an existing user by their ID")
            .WithOpenApi(operation =>
            {
                operation.Parameters[0].Description = "The unique identifier of the user to activate";
                operation.Responses["200"].Description = "User activated successfully";
                operation.Responses["404"].Description = "User not found";
                return operation;
            })
            .Produces<UserDto>(200)
            .Produces(404);

        group.MapGet("/", GetAllUsers)
            .WithName("GetAllUsers")
            .WithSummary("Get all users")
            .WithDescription("Retrieves a list of all users in the system")
            .WithOpenApi(operation =>
            {
                operation.Responses["200"].Description = "Successfully retrieved all users";
                return operation;
            })
            .Produces<IEnumerable<UserDto>>(200);

        return routes;
    }

    private static async Task<Results<Ok<UserDto>, NotFound>> GetUserById(
        [FromRoute] Guid id,
        IMediator mediator,
        CancellationToken cancellationToken
    )
    {
        var query = new GetUserByIdQuery(id);
        var user = await mediator.Send(query, cancellationToken);

        if (user is null)
            return TypedResults.NotFound();

        return TypedResults.Ok(user);
    }

    private static async Task<Results<Created<UserDto>, ValidationProblem>> CreateUser(
        [FromBody] CreateUserDto createUserDto,
        IMediator mediator,
        IUserRead userRead,
        CancellationToken cancellationToken
    )
    {
        var command = new CreateUserCommand
        {
            Email = createUserDto.Email,
            Name = createUserDto.Name
        };

        var user = await mediator.Send(command, cancellationToken);

        return TypedResults.Created($"/api/users/{user.Id}", user);
    }

    private static async Task<Results<Ok<UserDto>, NotFound>> DeactivateUser(
        [FromRoute] Guid id,
        IMediator mediator,
        CancellationToken cancellationToken
    )
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

    private static async Task<Results<Ok<UserDto>, NotFound>> ActivateUser(
        [FromRoute] Guid id,
        IMediator mediator,
        CancellationToken cancellationToken
    )
    {
        var command = new ActivateUserCommand { UserId = id };

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

    private static async Task<Ok<IEnumerable<UserDto>>> GetAllUsers(
        IMediator mediator,
        CancellationToken cancellationToken
    )
    {
        var query = new GetAllUsersQuery();
        var users = await mediator.Send(query, cancellationToken);
        return TypedResults.Ok(users);
    }
}