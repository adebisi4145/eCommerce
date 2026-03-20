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

            app.MapGet("/api/users/me", async (IMediator mediator) =>
            {
                var result = await mediator.Send(new GetProfileQuery());

                return Results.Ok(result);

            }).RequireAuthorization();

            app.MapPost("/api/users/addresses", async (AddAddressCommand command, IMediator mediator) =>
            {
                var result = await mediator.Send(command);
                return Results.Ok(result);

            }).RequireAuthorization();

            app.MapGet("/api/users/addresses", async (IMediator mediator) =>
            {
                var result = await mediator.Send(new GetAddressesQuery());
                return Results.Ok(result);

            }).RequireAuthorization();

            app.MapPut("/api/users/addresses/{addressId:guid}/default", async (Guid addressId, IMediator mediator) =>
            {
                await mediator.Send(new SetDefaultAddressCommand(addressId));
                return Results.NoContent();

            }).RequireAuthorization();

            app.MapDelete("/api/users/addresses/{addressId:guid}", async (Guid addressId, IMediator mediator) =>
            {
                await mediator.Send(new RemoveAddressCommand(addressId));
                return Results.NoContent();

            }).RequireAuthorization();

            app.MapPut("/api/users/addresses/{addressId:guid}", async (Guid addressId, UpdateAddressCommand command, IMediator mediator) =>
            {
                var updatedCommand = command with { AddressId = addressId };

                await mediator.Send(updatedCommand);

                return Results.NoContent();

            }).RequireAuthorization();
        }
    }
}
