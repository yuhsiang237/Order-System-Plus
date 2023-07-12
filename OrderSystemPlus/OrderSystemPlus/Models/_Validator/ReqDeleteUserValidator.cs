using FluentValidation;

using OrderSystemPlus.Models.BusinessActor;

public class ReqDeleteUserValidator : AbstractValidator<ReqDeleteUser>
{
    public ReqDeleteUserValidator()
    {
        RuleFor(x => x.Id)
            .NotNull().WithMessage("必填")
            .NotEmpty().WithMessage("必填");
    }
}