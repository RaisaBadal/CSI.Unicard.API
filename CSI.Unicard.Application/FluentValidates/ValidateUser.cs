using CSI.Unicard.Domain.Models;
using FluentValidation;

namespace CSI.Unicard.Application.FluentValidates
{
    public class ValidateUser:AbstractValidator<Users>
    {
        public ValidateUser()
        {
            RuleFor(i => i.Email).NotEmpty()
                .WithMessage("Email is required");
                
            RuleFor(i=>i.Password).NotEmpty()
                 .WithMessage("Password is required");

            RuleFor(i=>i.UserName).NotEmpty()
                 .WithMessage("UserName is required");

        }
    }
}
