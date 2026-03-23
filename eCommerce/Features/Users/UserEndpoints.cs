using eCommerce.Domain.Entities;
using eCommerce.Features.Users.AddAddress;
using eCommerce.Features.Users.GetAddresses;
using eCommerce.Features.Users.Login;
using eCommerce.Features.Users.Profile;
using eCommerce.Features.Users.Register;
using eCommerce.Features.Users.RemoveAddress;
using eCommerce.Features.Users.SetDefaultAddress;
using eCommerce.Features.Users.UpdateAddress;
using eCommerce.Infrastructure.Auth;
using MediatR;

namespace eCommerce.Features.Users
{
    public static class UserEndpoints
    {
        public static void MapUserEndpoints(this IEndpointRouteBuilder app)
        {
            var users = app.MapGroup("/api/users");

            users.MapPost("/register", async (RegisterUserCommand command, IMediator mediator) =>
            {
                var result = await mediator.Send(command);
                return Results.Ok(result);
            });

            users.MapPost("/login", async (LoginUserCommand command, IMediator mediator) =>
            {
                var result = await mediator.Send(command);
                return Results.Ok(result);
            });

            users.MapGet("/me", async (IMediator mediator) =>
            {
                var result = await mediator.Send(new GetProfileQuery());

                return Results.Ok(result);

            }).RequireAuthorization();

            var addresses = users.MapGroup("/addresses").RequireAuthorization();

            addresses.MapPost("/", async (AddAddressCommand command, IMediator mediator) =>
            {
                var result = await mediator.Send(command);
                return Results.Ok(result);

            });

            addresses.MapGet("/", async (IMediator mediator) =>
            {
                var result = await mediator.Send(new GetAddressesQuery());
                return Results.Ok(result);

            });

            addresses.MapPut("/{addressId:guid}/default", async (Guid addressId, IMediator mediator) =>
            {
                await mediator.Send(new SetDefaultAddressCommand(addressId));
                return Results.NoContent();

            });

            addresses.MapDelete("/{addressId:guid}", async (Guid addressId, IMediator mediator) =>
            {
                await mediator.Send(new RemoveAddressCommand(addressId));
                return Results.NoContent();

            });

            addresses.MapPut("/{addressId:guid}", async (Guid addressId, UpdateAddressCommand command, IMediator mediator) =>
            {
                var updatedCommand = command with { AddressId = addressId };

                await mediator.Send(updatedCommand);

                return Results.NoContent();

            });
        }
    }
}
