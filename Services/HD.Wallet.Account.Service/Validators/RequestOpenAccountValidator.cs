using FluentValidation;
using HD.Wallet.Account.Service.Dtos;

namespace HD.Wallet.Account.Service.Validators
{
    public class RequestOpenAccountValidator : AbstractValidator<RequestOpenAccountDto>
    {
        public RequestOpenAccountValidator()
        {
            RuleFor(exp => exp.PhoneNumber)
                .NotEmpty()
                .Matches("^(03|05|07|08|09)\\d{8}$")
                .WithMessage("PhoneNumber is invalid");

            RuleFor(exp => exp.IdCardNo)
                .NotEmpty()
                .WithMessage("IdCardNo must not be null")
                .Matches(@"^\d{9}$|^\d{12}$")
                .WithMessage("IdCardNo must be either 9 or 12 digits long.");

            RuleFor(exp => exp.FullName)
                .NotEmpty()
                .WithMessage("FullName must not be null")
                .WithMessage("FullName is invalid");

            RuleFor(exp => exp.DateOfBirth)
                .NotEmpty()
                .WithMessage("DateOfBirth must not be empty")
                .Must(date => date <= DateTime.Now.AddYears(-18))
                .WithMessage("You must be at least 18 years old.");

            RuleFor(exp => exp.Email)
                .NotEmpty()
                .WithMessage("Email must not be null")
                .EmailAddress()
                .WithMessage("Email is invalid");

            RuleFor(exp => exp.Password)
                .NotEmpty()
                .WithMessage("Password must not be empty")
                .MinimumLength(8)
                .WithMessage("Password must be at least 8 characters long")
                .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter")
                .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter")
                .Matches(@"\d").WithMessage("Password must contain at least one digit")
                .Matches(@"[\W_]").WithMessage("Password must contain at least one special character");
        }
    }
}
