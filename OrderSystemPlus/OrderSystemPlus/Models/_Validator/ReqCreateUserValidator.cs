using FluentValidation;
using OrderSystemPlus.Models.BusinessActor;

public class ReqCreateUserValidator : AbstractValidator<ReqCreateUser>
{
    public ReqCreateUserValidator()
    {
        RuleFor(x => x.Account)
            .NotNull().WithMessage("必填")
            .NotEmpty().WithMessage("必填");
        RuleFor(x => x.Password)
            .NotNull().WithMessage("必填")
            .NotEmpty().WithMessage("必填");
        RuleFor(x => x.Email)
            .NotNull().WithMessage("必填")
            .NotEmpty().WithMessage("必填");
        RuleFor(x => x.Name)
            .NotNull().WithMessage("必填")
            .NotEmpty().WithMessage("必填");
    }
}