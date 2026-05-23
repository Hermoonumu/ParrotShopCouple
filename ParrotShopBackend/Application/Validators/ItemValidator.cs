using FluentValidation;
using ParrotShopBackend.Application.DTO;
using ParrotShopBackend.Domain;

namespace ParrotShopBackend.Application.Validators;



public class ItemValidator : AbstractValidator<ItemDTO>
{
    public ItemValidator()
    {
        RuleFor(x => x.Name)
        .NotEmpty().WithMessage("Name cannot be empty.");

        RuleFor(x => x.Price)
        .GreaterThan(0).WithMessage("Price must be greater than 0.");


    }
}