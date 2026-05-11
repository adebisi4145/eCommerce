using FluentValidation;

namespace eCommerce.Features.Cart.UpdateCartItem
{
    public class UpdateCartItemValidator : AbstractValidator<UpdateCartItemCommand>
    {
        public UpdateCartItemValidator()
        {
            RuleFor(x => x.Quantity).GreaterThan(0);
        }
    }
}
