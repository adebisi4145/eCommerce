using FluentValidation;

namespace eCommerce.Features.Users.Login
{
    public class LoginUserValidator: AbstractValidator<LoginUserCommand>
    {
        public LoginUserValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty();
        }
    }
}
