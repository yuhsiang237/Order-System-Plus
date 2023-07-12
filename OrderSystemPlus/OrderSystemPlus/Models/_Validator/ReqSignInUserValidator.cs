using FluentValidation;

using OrderSystemPlus.Models.BusinessActor;

public class ReqSignInUserValidator : AbstractValidator<ReqSignInUser>
{
    public ReqSignInUserValidator()
    {
        RuleFor(x => x.Account)
            .NotNull().WithMessage("必填")
            .NotEmpty().WithMessage("必填");
        RuleFor(x => x.Password)
            .NotNull().WithMessage("必填")
            .NotEmpty().WithMessage("必填");
    }
}