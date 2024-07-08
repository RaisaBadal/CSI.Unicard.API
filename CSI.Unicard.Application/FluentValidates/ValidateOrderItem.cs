using CSI.Unicard.Domain.Models;
using FluentValidation;

namespace CSI.Unicard.Application.FluentValidates
{
    public class ValidateOrderItem:AbstractValidator<OrderItems>
    {
        public ValidateOrderItem()
        {
            RuleFor(i => i.OrderId).NotEmpty();

            RuleFor(i => i.ProductId).NotEmpty();

            RuleFor(i => i.price)
                 .NotEmpty()
                 .WithMessage("Price is required")
                 .GreaterThan(0);

            RuleFor(i => i.Quantity)
                 .NotEmpty()
                 .GreaterThan(0);

        }
    }
}
