namespace eCommerce.Domain.Entities
{
    public class OrderItem
    {
        public Guid ProductId { get; private set; }
        public string ProductName { get; private set; } = string.Empty;
        public decimal UnitPrice { get; private set; }
        public int Quantity { get; private set; }
        public decimal Subtotal => UnitPrice * Quantity;

        private OrderItem() { }

        public OrderItem(Guid productId, string productName, decimal unitPrice, int quantity)
        {
            ProductId = productId;
            ProductName = productName;
            UnitPrice = unitPrice;
            Quantity = quantity;
        }
    }
}
