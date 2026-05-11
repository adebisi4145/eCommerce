namespace eCommerce.Features.Categories.GetCategories
{
    public record CategorySummary(Guid Id, string Name);

    public record GetCategoriesResponse(IEnumerable<CategorySummary> Categories);
}
