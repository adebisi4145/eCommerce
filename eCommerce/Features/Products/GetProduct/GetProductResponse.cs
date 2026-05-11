namespace eCommerce.Features.Products.GetProduct
{
    public record GetProductResponse(
        Guid Id,
        string Name,
        string Description,
        decimal Price,
        int Stock,
        bool IsActive,
        Guid CategoryId,
        string CategoryName);
}
