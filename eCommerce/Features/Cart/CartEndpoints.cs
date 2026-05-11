using eCommerce.Features.Cart.AddCartItem;
using eCommerce.Features.Cart.ClearCart;
using eCommerce.Features.Cart.GetCart;
using eCommerce.Features.Cart.RemoveCartItem;
using eCommerce.Features.Cart.UpdateCartItem;
using MediatR;

namespace eCommerce.Features.Cart
{
    public static class CartEndpoints
    {
        public static void MapCartEndpoints(this IEndpointRouteBuilder app)
        {
            var cart = app.MapGroup("/api/cart").RequireAuthorization();

            cart.MapGet("/", async (IMediator mediator) =>
            {
                var result = await mediator.Send(new GetCartQuery());
                return Results.Ok(result);
            });

            cart.MapPost("/items", async (AddCartItemCommand command, IMediator mediator) =>
            {
                await mediator.Send(command);
                return Results.NoContent();
            });

            cart.MapPut("/items/{productId:guid}", async (Guid productId, UpdateCartItemCommand command, IMediator mediator) =>
            {
                var updatedCommand = command with { ProductId = productId };
                await mediator.Send(updatedCommand);
                return Results.NoContent();
            });

            cart.MapDelete("/items/{productId:guid}", async (Guid productId, IMediator mediator) =>
            {
                await mediator.Send(new RemoveCartItemCommand(productId));
                return Results.NoContent();
            });

            cart.MapDelete("/", async (IMediator mediator) =>
            {
                await mediator.Send(new ClearCartCommand());
                return Results.NoContent();
            });
        }
    }
}
