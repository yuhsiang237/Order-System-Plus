﻿using FluentValidation;
using OrderSystemPlus.Models.BusinessActor;

public class ReqUpdateProductValidator : AbstractValidator<ReqUpdateProduct>
{
    public ReqUpdateProductValidator()
    {
        RuleFor(x => x.Name)
             .NotNull().WithMessage("必填")
             .NotEmpty().WithMessage("必填");

        RuleFor(x => x.Number)
         .NotNull().WithMessage("必填")
         .NotEmpty().WithMessage("必填");

        RuleFor(x => x.Price)
        .NotNull().WithMessage("必填")
        .GreaterThanOrEqualTo(0).WithMessage("不可為負");
    }
}