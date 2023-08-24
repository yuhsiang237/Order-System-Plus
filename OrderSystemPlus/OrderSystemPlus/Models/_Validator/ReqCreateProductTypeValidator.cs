using FluentValidation;
using OrderSystemPlus.Models.BusinessActor;

public class ReqCreateProductValidator : AbstractValidator<ReqCreateProduct>
{
    public ReqCreateProductValidator()
    {
        RuleFor(x => x.Name)
            .NotNull().WithMessage("必填")
            .NotEmpty().WithMessage("必填");

        RuleFor(x => x.Number)
         .NotNull().WithMessage("必填")
         .NotEmpty().WithMessage("必填");

        RuleFor(x => x.Price)
        .NotNull().WithMessage("必填")
        .NotEmpty().WithMessage("必填")
        .GreaterThanOrEqualTo(0);

        RuleFor(x => x.Quantity)
        .NotNull().WithMessage("必填")
        .NotEmpty().WithMessage("必填")
        .GreaterThanOrEqualTo(0);
    }
}