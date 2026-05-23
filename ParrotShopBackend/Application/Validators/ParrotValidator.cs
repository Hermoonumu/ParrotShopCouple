using FluentValidation;
using ParrotShopBackend.Application.DTO;

namespace ParrotShopBackend.Application.Validators;



public class ParrotValidator : AbstractValidator<NewParrotDTO>
{
    public ParrotValidator()
    {
        RuleFor(x => x.Name)
        .NotEmpty().WithMessage("Name cannot be empty.");

        RuleFor(x => x.Price)
        .GreaterThan(0).WithMessage("Price must be greater than 0.");
        RuleFor(x => x.Discount)
        .GreaterThanOrEqualTo(0).WithMessage("Discount must be greater or equal than 0.");

        RuleFor(x => x.Age)
        .Must(age => age > 0).WithMessage("Age must be greater than 0.");

        RuleFor(x => x.ColorType)
        .NotEmpty().WithMessage("At least one color must be specified.");

        RuleFor(x => x.GenderType)
        .IsInEnum().WithMessage("Gender must be specified.");

        RuleFor(x => x.SpeciesType)
        .NotEmpty().WithMessage("Species must be specified.");



    }
}