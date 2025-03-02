using FastEndpoints;
using Microsoft.AspNetCore.Mvc;

namespace Api.Users.v1.GetUserById;

public class GetUserByIdRequest
{
 [FromRoute]   public Guid Id { get; set; }
}