namespace eCommerce.Features.Cart.GetCart
{
    public record CartItemDto(Guid ProductId, string ProductName, decimal UnitPrice, int Quantity, decimal Subtotal);

    public record GetCartResponse(Guid CartId, IEnumerable<CartItemDto> Items, decimal Total);
}
