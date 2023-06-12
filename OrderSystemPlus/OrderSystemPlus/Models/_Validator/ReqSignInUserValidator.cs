using FluentValidation;

using OrderSystemPlus.Models.BusinessActor;

public class ReqSignInUserValidator : AbstractValidator<ReqSignInUser>
{
    public ReqSignInUserValidator()
    {
        RuleFor(x => x.Account)
            .NotNull().WithMessage("不可為空")
            .NotEmpty().WithMessage("不可為空");
        RuleFor(x => x.Password)
            .NotNull()
            .WithMessage("不可為空")
            .NotEmpty()
            .WithMessage("不可為空");
    }
}