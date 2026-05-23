using FluentValidation;
using ParrotShopBackend.Application.DTO;

namespace ParrotShopBackend.Application.Validators;



public class LoginValidator : AbstractValidator<LoginFormDTO>
{

    public LoginValidator()
    {
        RuleFor(x => x.Username)
        .NotEmpty().WithMessage("Username cannot be empty.")
        .MaximumLength(256).WithMessage("Username cannot be longer than 256 characters.")
        .MinimumLength(8).WithMessage("Username must be at least 8 characters long.");

        RuleFor(x => x.Password)
        .NotEmpty().WithMessage("Password cannot be empty.")
        .MinimumLength(8)
        .MaximumLength(1024);
    }
}