using FluentValidation;
using OrderSystemPlus.Models.BusinessActor;

public class ReqUpdateProductTypeValidator : AbstractValidator<ReqUpdateProductType>
{
    public ReqUpdateProductTypeValidator()
    {
        RuleFor(x => x.Name)
            .NotNull().WithMessage("必填")
            .NotEmpty().WithMessage("必填");
    }
}