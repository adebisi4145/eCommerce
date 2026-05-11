using eCommerce.Features.Products.CreateProduct;
using eCommerce.Features.Products.DeactivateProduct;
using eCommerce.Features.Products.GetProduct;
using eCommerce.Features.Products.GetProducts;
using eCommerce.Features.Products.UpdateProduct;
using eCommerce.Features.Products.UpdateStock;
using MediatR;

namespace eCommerce.Features.Products
{
    public static class ProductEndpoints
    {
        public static void MapProductEndPoints(this IEndpointRouteBuilder app)
        {
            var products = app.MapGroup("/api/products");

            products.MapGet("/", async (Guid? categoryId, int page, int pageSize, IMediator mediator) =>
            {
                var result = await mediator.Send(new GetProductsQuery(categoryId, page <= 0 ? 1 : page, pageSize <= 0 ? 20 : pageSize));
                return Results.Ok(result);
            });

            products.MapGet("/{id:guid}", async (Guid id, IMediator mediator) =>
            {
                var result = await mediator.Send(new GetProductQuery(id));
                return Results.Ok(result);
            });

            products.MapPost("/", async (CreateProductCommand command, IMediator mediator) =>
            {
                var result = await mediator.Send(command);
                return Results.Created($"/api/products/{result.ProductId}", result);
            }).RequireAuthorization("AdminPolicy");

            products.MapPut("/{id:guid}", async (Guid id, UpdateProductCommand command, IMediator mediator) =>
            {
                var updatedCommand = command with { Id = id };
                await mediator.Send(updatedCommand);
                return Results.NoContent();
            }).RequireAuthorization("AdminPolicy");

            products.MapPut("/{id:guid}/stock", async (Guid id, UpdateStockCommand command, IMediator mediator) =>
            {
                var updatedCommand = command with { Id = id };
                await mediator.Send(updatedCommand);
                return Results.NoContent();
            }).RequireAuthorization("AdminPolicy");

            products.MapDelete("/{id:guid}", async (Guid id, IMediator mediator) =>
            {
                await mediator.Send(new DeactivateProductCommand(id));
                return Results.NoContent();
            }).RequireAuthorization("AdminPolicy");
        }
    }
}
