using eCommerce.Domain.Enums;

namespace eCommerce.Domain.Entities
{
    public class Order
    {
        public Guid Id { get; private set; }
        public Guid UserId { get; private set; }
        public OrderStatus Status { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public decimal TotalAmount { get; private set; }

        public string ShippingStreet { get; private set; } = string.Empty;
        public string ShippingCity { get; private set; } = string.Empty;
        public string ShippingState { get; private set; } = string.Empty;
        public string ShippingCountry { get; private set; } = string.Empty;
        public string ShippingZipCode { get; private set; } = string.Empty;

        private readonly List<OrderItem> _items = new();
        public IReadOnlyCollection<OrderItem> Items => _items;

        private Order() { }

        public Order(Guid userId, string street, string city, string state, string country, string zipCode)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            Status = OrderStatus.Pending;
            CreatedAt = DateTime.UtcNow;
            ShippingStreet = street;
            ShippingCity = city;
            ShippingState = state;
            ShippingCountry = country;
            ShippingZipCode = zipCode;
        }

        public void AddItem(Guid productId, string productName, decimal unitPrice, int quantity)
        {
            _items.Add(new OrderItem(productId, productName, unitPrice, quantity));
            TotalAmount += unitPrice * quantity;
        }

        public void Cancel()
        {
            if (Status == OrderStatus.Shipped || Status == OrderStatus.Delivered)
                throw new InvalidOperationException("Order cannot be cancelled once it has been shipped");

            if (Status == OrderStatus.Cancelled)
                throw new InvalidOperationException("Order is already cancelled");

            Status = OrderStatus.Cancelled;
        }

        public void UpdateStatus(OrderStatus status)
        {
            if (Status == OrderStatus.Cancelled)
                throw new InvalidOperationException("Cannot update a cancelled order");

            if (Status == OrderStatus.Delivered)
                throw new InvalidOperationException("Cannot update a delivered order");

            Status = status;
        }
    }
}
