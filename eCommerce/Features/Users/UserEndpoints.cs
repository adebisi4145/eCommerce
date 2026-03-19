using eCommerce.Features.Users.Login;
using eCommerce.Features.Users.Register;
using MediatR;

namespace eCommerce.Features.Users
{
    public static class UserEndpoints
    {
        public static void MapUserEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("/api/users/register", async (RegisterUserCommand command, IMediator mediator) =>
            {
                var result = await mediator.Send(command);
                return Results.Ok(result);
            });

            app.MapPost("/api/users/login", async (LoginUserCommand command, IMediator mediator) =>
            {
                var result = await mediator.Send(command);
                return Results.Ok(result);
            });
        }
    }
}
