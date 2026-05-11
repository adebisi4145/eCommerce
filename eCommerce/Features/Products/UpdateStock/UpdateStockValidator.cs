using FluentValidation;

namespace eCommerce.Features.Products.UpdateStock
{
    public class UpdateStockValidator : AbstractValidator<UpdateStockCommand>
    {
        public UpdateStockValidator()
        {
            RuleFor(x => x.Stock).GreaterThanOrEqualTo(0);
        }
    }
}
