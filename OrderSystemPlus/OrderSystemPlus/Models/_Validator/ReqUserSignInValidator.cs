using FluentValidation;

using OrderSystemPlus.Models.BusinessActor.Commands;

public class ReqUserSignInValidator : AbstractValidator<ReqUserSignIn>
{
    public ReqUserSignInValidator()
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