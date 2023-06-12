using FluentValidation;
using OrderSystemPlus.Models.BusinessActor;

public class ReqCreateUserValidator : AbstractValidator<ReqCreateUser>
{
    public ReqCreateUserValidator()
    {
        RuleFor(x => x.Account).NotNull().NotEmpty().WithMessage("必填");
        RuleFor(x => x.Password).NotNull().NotEmpty().WithMessage("必填");
        RuleFor(x => x.Email).NotNull().NotEmpty().WithMessage("必填");
        RuleFor(x => x.Name).NotNull().NotEmpty().WithMessage("必填");
    }
}