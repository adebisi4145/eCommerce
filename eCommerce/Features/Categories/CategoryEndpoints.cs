using eCommerce.Features.Categories.CreateCategory;
using eCommerce.Features.Categories.DeleteCategory;
using eCommerce.Features.Categories.GetCategories;
using eCommerce.Features.Categories.GetCategory;
using eCommerce.Features.Categories.UpdateCategory;
using MediatR;

namespace eCommerce.Features.Categories
{
    public static class CategoryEndpoints
    {
        public static void MapCategoryEndpoints(this IEndpointRouteBuilder app)
        {
            var categories = app.MapGroup("/api/categories");

            categories.MapGet("/", async (IMediator mediator) =>
            {
                var result = await mediator.Send(new GetCategoriesQuery());
                return Results.Ok(result);
            });

            categories.MapGet("/{id:guid}", async (Guid id, IMediator mediator) =>
            {
                var result = await mediator.Send(new GetCategoryQuery(id));
                return Results.Ok(result);
            });

            categories.MapPost("/", async (CreateCategoryCommand command, IMediator mediator) =>
            {
                var result = await mediator.Send(command);
                return Results.Created($"/api/categories/{result.CategoryId}", result);
            }).RequireAuthorization("AdminPolicy");

            categories.MapPut("/{id:guid}", async (Guid id, UpdateCategoryCommand command, IMediator mediator) =>
            {
                var updatedCommand = command with { Id = id };
                await mediator.Send(updatedCommand);
                return Results.NoContent();
            }).RequireAuthorization("AdminPolicy");

            categories.MapDelete("/{id:guid}", async (Guid id, IMediator mediator) =>
            {
                await mediator.Send(new DeleteCategoryCommand(id));
                return Results.NoContent();
            }).RequireAuthorization("AdminPolicy");
        }
    }
}
