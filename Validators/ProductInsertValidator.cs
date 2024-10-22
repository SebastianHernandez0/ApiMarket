using ApiMarket.DTOs;
using FluentValidation;

namespace ApiMarket.Validators
{
    public class ProductInsertValidator : AbstractValidator<ProductoRequestDto>
    {
        public ProductInsertValidator()
        {
            RuleFor(x=>x.Name).NotEmpty().MinimumLength(3).WithMessage("El nombre es requerido");
            RuleFor(x=>x.Price).GreaterThan(0).WithMessage("El precio debe ser mayor a 0");
            RuleFor(x => x.Stock).GreaterThan(0).WithMessage("El stock debe ser mayor a 0");
            RuleFor(x=> x.CategoryId).NotEmpty().WithMessage("La categoria es requerida");
        }


    }
}
