using FluentValidation;
using OrderSystemPlus.Models.BusinessActor;

public class ReqCreateProductTypeValidator : AbstractValidator<ReqCreateProductType>
{
    public ReqCreateProductTypeValidator()
    {
        RuleFor(x => x.Name)
            .NotNull().WithMessage("必填")
            .NotEmpty().WithMessage("必填");
    }
}