namespace eCommerce.Domain.Entities
{
    public class Product
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public decimal Price { get; private set; }
        public int Stock { get; private set; }
        public bool IsActive { get; private set; }

        public Guid CategoryId { get; private set; }
        public Category Category { get; private set; } = null!;

        private Product() { } // EF

        public Product(string name, string description, decimal price, int stock, Guid categoryId)
        {
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            Price = price;
            CategoryId = categoryId;
            Stock = stock;
            IsActive = true;
        }

        public void UpdateDetails(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public void UpdatePrice(decimal price)
        {
            if (price <= 0)
                throw new InvalidOperationException("Price must be greater than zero");

            Price = price;
        }

        public void Deactivate()
        {
            IsActive = false;
        }

        public void Activate()
        {
            IsActive = true;
        }

        public void SetStock(int stock)
        {
            if (stock < 0)
                throw new InvalidOperationException("Stock cannot be negative");

            Stock = stock;
        }
    }
}
