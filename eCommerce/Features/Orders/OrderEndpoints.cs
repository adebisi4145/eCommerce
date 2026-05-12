using eCommerce.Features.Orders.CancelOrder;
using eCommerce.Features.Orders.GetOrder;
using eCommerce.Features.Orders.GetOrders;
using eCommerce.Features.Orders.PlaceOrder;
using eCommerce.Features.Orders.UpdateOrderStatus;
using MediatR;

namespace eCommerce.Features.Orders
{
    public static class OrderEndpoints
    {
        public static void MapOrderEndpoints(this IEndpointRouteBuilder app)
        {
            var orders = app.MapGroup("/api/orders").RequireAuthorization();

            orders.MapGet("/", async (IMediator mediator) =>
            {
                var result = await mediator.Send(new GetOrdersQuery());
                return Results.Ok(result);
            });

            orders.MapGet("/{id:guid}", async (Guid id, IMediator mediator) =>
            {
                var result = await mediator.Send(new GetOrderQuery(id));
                return Results.Ok(result);
            });

            orders.MapPost("/", async (PlaceOrderCommand command, IMediator mediator) =>
            {
                var result = await mediator.Send(command);
                return Results.Created($"/api/orders/{result.OrderId}", result);
            });

            orders.MapPut("/{id:guid}/cancel", async (Guid id, IMediator mediator) =>
            {
                await mediator.Send(new CancelOrderCommand(id));
                return Results.NoContent();
            });

            orders.MapPut("/{id:guid}/status", async (Guid id, UpdateOrderStatusCommand command, IMediator mediator) =>
            {
                var updatedCommand = command with { Id = id };
                await mediator.Send(updatedCommand);
                return Results.NoContent();
            }).RequireAuthorization("AdminPolicy");
        }
    }
}
