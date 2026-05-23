using System.ComponentModel.DataAnnotations;
using FluentValidation;
using ParrotShopBackend.Application.DTO;

namespace ParrotShopBackend.Application.Validators;



public class RegisterValidator : AbstractValidator<RegFormDTO>
{
    public RegisterValidator()
    {
        RuleFor(x => x.Name)
        .NotEmpty().WithMessage("Name cannot be empty.")
        .MaximumLength(256).WithMessage("Name cannot be longer than 256 characters.");

        RuleFor(x => x.Username)
        .NotEmpty().WithMessage("Username cannot be empty.")
        .MaximumLength(256).WithMessage("Username cannot be longer than 256 characters.")
        .MinimumLength(8).WithMessage("Username must be at least 8 characters long.")
        .Must(name => name == null || !name.Contains("admin", StringComparison.OrdinalIgnoreCase))
        .WithMessage("You cannot have 'admin' in your name.");

        RuleFor(x => x.Email)
        .EmailAddress().NotEmpty().WithMessage("Email cannot be empty.");

        RuleFor(x => x.Password)
        .NotEmpty().WithMessage("Password cannot be empty.")
        .MinimumLength(8)
        .MaximumLength(1024)
        .Matches("[A-Z]").WithMessage("Password must contain one uppercase letter")
        .Matches("[a-z]").WithMessage("Password must contain one lowercase letter")
        .Matches("[0-9]").WithMessage("Password must contain one number");

        RuleFor(x => x.Phone)
        .NotEmpty().WithMessage("Phone cannot be empty.")
        .MaximumLength(64).WithMessage("Phone cannot be longer than 64 characters.")
        .MinimumLength(8).WithMessage("Phone must be at least 8 characters long.")
        .Must(p => p.Contains('+')).WithMessage("Phone must start with '+'");

        RuleFor(x => x.ShippingAddress)
        .NotEmpty().WithMessage("Shipping address cannot be empty.")
        .MaximumLength(256).WithMessage("Shipping address cannot be longer than 256 characters.");

    }
}