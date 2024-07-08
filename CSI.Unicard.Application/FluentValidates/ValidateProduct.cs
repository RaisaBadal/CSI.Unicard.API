using CSI.Unicard.Application.DTOs;
using FluentValidation;

namespace CSI.Unicard.Application.FluentValidates
{
    public class ValidateProduct:AbstractValidator<ProductDTO>
    {
        public ValidateProduct()
        {
            RuleFor(i=>i.ProductName)
                .NotEmpty()
                .WithMessage("ProductName is required");

            RuleFor(i => i.Price)
                .NotEmpty()
                .WithMessage("Price is required")
                .GreaterThan(0)
                .WithMessage("Price must be greater than zero.");
               

            RuleFor(i => i.Description)
                .NotEmpty()
                .WithMessage("Description is required");
        }
    }
}
