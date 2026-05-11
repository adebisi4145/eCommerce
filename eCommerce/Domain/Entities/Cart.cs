namespace eCommerce.Domain.Entities
{
    public class Cart
    {
        public Guid Id { get; private set; }
        public Guid UserId { get; private set; }

        private readonly List<CartItem> _items = new();
        public IReadOnlyCollection<CartItem> Items => _items;

        private Cart() { }

        public Cart(Guid userId)
        {
            Id = Guid.NewGuid();
            UserId = userId;
        }

        public void AddItem(Guid productId, int quantity)
        {
            var existing = _items.FirstOrDefault(i => i.ProductId == productId);
            if (existing != null)
                existing.UpdateQuantity(existing.Quantity + quantity);
            else
                _items.Add(new CartItem(productId, quantity));
        }

        public void UpdateItemQuantity(Guid productId, int quantity)
        {
            var item = _items.FirstOrDefault(i => i.ProductId == productId);
            if (item == null)
                throw new KeyNotFoundException("Item not found in cart");

            item.UpdateQuantity(quantity);
        }

        public void RemoveItem(Guid productId)
        {
            var item = _items.FirstOrDefault(i => i.ProductId == productId);
            if (item == null)
                throw new KeyNotFoundException("Item not found in cart");

            _items.Remove(item);
        }

        public void Clear() => _items.Clear();
    }
}
