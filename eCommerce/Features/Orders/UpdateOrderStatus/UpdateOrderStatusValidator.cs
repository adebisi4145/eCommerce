using eCommerce.Domain.Enums;
using FluentValidation;

namespace eCommerce.Features.Orders.UpdateOrderStatus
{
    public class UpdateOrderStatusValidator : AbstractValidator<UpdateOrderStatusCommand>
    {
        private static readonly string[] ValidStatuses = Enum.GetNames<OrderStatus>();

        public UpdateOrderStatusValidator()
        {
            RuleFor(x => x.Status)
                .NotEmpty()
                .Must(s => ValidStatuses.Contains(s, StringComparer.OrdinalIgnoreCase))
                .WithMessage($"Status must be one of: {string.Join(", ", ValidStatuses)}");
        }
    }
}
