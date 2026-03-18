using System.Reflection.Emit;

namespace eCommerce.Domain.Entities
{
    public class Address
    {
        public Guid Id { get; set; }
        public string Street { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;
        public bool IsDefault { get; private set; }
        private Address() { } 
        public Address(string street, string city, string state, string country, string zipCode, bool isDefault = false)
        {
            Id = Guid.NewGuid();
            Street = street;
            City = city;
            State = state;
            Country = country;
            IsDefault = isDefault;
            ZipCode = zipCode;
        }

        public void Update(string street, string city, string state, string country, string zipCode)
        {
            Street = street;
            City = city;
            State = state;
            Country = country;
            ZipCode = zipCode;
        }

        public void SetAsDefault()
        {
            IsDefault = true;
        }

        public void RemoveDefault()
        {
            IsDefault = false;
        }
    }
}
