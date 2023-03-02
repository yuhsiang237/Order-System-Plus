using FluentValidation;

using OrderSystemPlus.Models.BusinessActor.Commands;

public class ReqUserUpdateValidator : AbstractValidator<ReqUserUpdate>
{
    public ReqUserUpdateValidator()
    {
        RuleFor(x => x.Id)
          .NotNull().WithMessage("不可為空")
          .NotEmpty().WithMessage("不可為空");
        RuleFor(x => x.Name)
            .NotNull().WithMessage("不可為空")
            .NotEmpty().WithMessage("不可為空");
        RuleFor(x => x.Email)
            .NotNull()
            .WithMessage("不可為空")
            .NotEmpty()
            .WithMessage("不可為空");
    }
}