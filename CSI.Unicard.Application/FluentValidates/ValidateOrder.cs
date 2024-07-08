using CSI.Unicard.Domain.Models;
using FluentValidation;

namespace CSI.Unicard.Application.FluentValidates
{
    public class ValidateOrder:AbstractValidator<Orders>
    {
        public ValidateOrder()
        {
            RuleFor(i => i.UserId).NotEmpty()
                .WithMessage("User ID is required.");
            RuleFor(i => i.OrderDate).NotEmpty()
                .WithMessage("OrderDate is required");
            RuleFor(i => i.TotalAmount).NotEmpty()
                .WithMessage("Total amount is required.")
                .GreaterThan(0)
                .WithMessage("Total amount must be greater than zero.");
        }
    }
}
