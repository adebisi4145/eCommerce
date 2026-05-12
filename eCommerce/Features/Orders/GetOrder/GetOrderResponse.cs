namespace eCommerce.Features.Orders.GetOrder
{
    public record OrderItemDto(Guid ProductId, string ProductName, decimal UnitPrice, int Quantity, decimal Subtotal);

    public record ShippingAddressDto(string Street, string City, string State, string Country, string ZipCode);

    public record GetOrderResponse(
        Guid Id,
        string Status,
        DateTime CreatedAt,
        decimal TotalAmount,
        ShippingAddressDto ShippingAddress,
        IEnumerable<OrderItemDto> Items);
}
