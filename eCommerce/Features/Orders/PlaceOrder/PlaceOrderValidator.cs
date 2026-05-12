using FluentValidation;

namespace eCommerce.Features.Orders.PlaceOrder
{
    public class PlaceOrderValidator : AbstractValidator<PlaceOrderCommand>
    {
        public PlaceOrderValidator()
        {
            RuleFor(x => x.ShippingAddressId).NotEmpty();
        }
    }
}
