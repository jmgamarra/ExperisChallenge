using FluentValidation;
using ProductManager.Api.DTOs;

namespace ProductManager.Api.Models
{
    public class AddProductValidator : AbstractValidator<AddProductDto>
    {
        public AddProductValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("El nombre del producto es requerido.")
                .MaximumLength(100).WithMessage("El nombre no debe exceder los 100 caracteres.");

            RuleFor(p => p.Price)
                .GreaterThan(0).WithMessage("El precio debe ser mayor a 0.");

            RuleFor(p => p.Quantity)
                .GreaterThanOrEqualTo(0).WithMessage("La cantidad no puede ser negativa.");

        }
    }
}
