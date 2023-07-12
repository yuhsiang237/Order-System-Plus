using FluentValidation;

using OrderSystemPlus.Models.BusinessActor;

public class ReqUpdateUserValidator : AbstractValidator<ReqUpdateUser>
{
    public ReqUpdateUserValidator()
    {
        RuleFor(x => x.Id)
            .NotNull().WithMessage("必填")
            .NotEmpty().WithMessage("必填");
        RuleFor(x => x.Name)
            .NotNull().WithMessage("必填")
            .NotEmpty().WithMessage("必填");
        RuleFor(x => x.Email)
            .NotNull().WithMessage("必填")
            .NotEmpty().WithMessage("必填");
    }
}