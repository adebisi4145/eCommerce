using FluentValidation;

namespace eCommerce.Features.Users.AddAddress
{
    public class AddAddressValidator: AbstractValidator<AddAddressCommand>
    {
        public AddAddressValidator()
        {
            RuleFor(x => x.Street).NotEmpty();
            RuleFor(x => x.City).NotEmpty();
            RuleFor(x => x.State).NotEmpty();
            RuleFor(x => x.Country).NotEmpty();
            RuleFor(x => x.ZipCode).NotEmpty();
        }
    }
}
