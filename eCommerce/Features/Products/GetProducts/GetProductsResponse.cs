namespace eCommerce.Features.Products.GetProducts
{
    public record ProductSummary(
        Guid Id,
        string Name,
        string Description,
        decimal Price,
        int Stock,
        bool IsActive,
        Guid CategoryId,
        string CategoryName);

    public record GetProductsResponse(
        IEnumerable<ProductSummary> Products,
        int TotalCount,
        int Page,
        int PageSize);
}
