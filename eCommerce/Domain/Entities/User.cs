namespace eCommerce.Domain.Entities
{
    public class User
    {
        public Guid Id { get; private set; }
        public string FirstName { get; private set; } = string.Empty;
        public string LastName { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string PasswordHash { get; private set; } = string.Empty;
        public string Role { get; private set; } = "User";
        private readonly List<Address> _addresses = new();
        public IReadOnlyCollection<Address> Addresses => _addresses;

        private User() { }

        public User(string email, string passwordHash, string firstName, string lastName, string role = "User")
        {
            Id = Guid.NewGuid();
            FirstName = firstName;
            LastName = lastName;
            Email = email.ToLowerInvariant();
            PasswordHash = passwordHash;
            Role = role;
        }

        public void AddAddress(Address address)
        {
            if (!_addresses.Any())
            {
                address.SetAsDefault();
            }
            else if (address.IsDefault)
            {
                foreach (var addr in _addresses)
                    addr.RemoveDefault();
            }

            _addresses.Add(address);
        }

        public void UpdateAddress(Guid addressId, string street, string city, string state, string country, string zipCode)
        {
            var address = _addresses.FirstOrDefault(a => a.Id == addressId)
                ?? throw new KeyNotFoundException("Address not found");

            address.Update(street, city, state, country, zipCode);
        }

        public void RemoveAddress(Guid addressId)
        {
            var address = _addresses.FirstOrDefault(a => a.Id == addressId)
            ?? throw new KeyNotFoundException("Address not found");

            var wasDefault = address.IsDefault;

            _addresses.Remove(address);

            if (wasDefault && _addresses.Any())
            {
                _addresses.First().SetAsDefault(); 
            }
        }

        public void SetDefaultAddress(Guid addressId)
        {
            var address = _addresses.FirstOrDefault(a => a.Id == addressId)
                ?? throw new KeyNotFoundException("Address not found");

            foreach (var addr in _addresses)
                addr.RemoveDefault();

            address.SetAsDefault();
        }
    }
}
