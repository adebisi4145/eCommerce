namespace eCommerce.Domain.Entities
{
    public class CartItem
    {
        public Guid ProductId { get; private set; }
        public int Quantity { get; private set; }

        private CartItem() { }

        public CartItem(Guid productId, int quantity)
        {
            ProductId = productId;
            Quantity = quantity;
        }

        public void UpdateQuantity(int quantity)
        {
            if (quantity <= 0)
                throw new InvalidOperationException("Quantity must be greater than zero");

            Quantity = quantity;
        }
    }
}
