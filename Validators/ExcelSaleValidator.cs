using FluentValidation;
using IKT_BACKEND.Dtos;

namespace IKT_BACKEND.Validators
{
    public class ExcelSaleValidator : AbstractValidator<ExcelSaleDto>
    {
        public ExcelSaleValidator()
        {
            RuleFor(x => x.DateTime)
                .NotEmpty().WithMessage("DateTime is required.");

            RuleFor(x => x.PaymentType)
                .IsInEnum().WithMessage("Invalid payment type.");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than zero.");

            RuleFor(x => x.ProductName)
                .NotEmpty().WithMessage("Product name is required.")
                .MaximumLength(100).WithMessage("Product name cannot exceed 100 characters.");

            RuleFor(x => x.CardNumber).MaximumLength(19).WithMessage("Card number cannot exceed 20 characters.");

            RuleFor(x => x.Month).GreaterThan(0).LessThanOrEqualTo(12).WithMessage("Month must be between 1 and 12.");
        }
    }
}   
