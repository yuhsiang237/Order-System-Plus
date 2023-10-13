using FluentValidation;
using OrderSystemPlus.Models.BusinessActor;

public class ReqUpdateProductInventoryValidator : AbstractValidator<List<ReqUpdateProductInventory>>
{
    public ReqUpdateProductInventoryValidator()
    {
        RuleForEach(x => x).SetValidator(new UpdateProductInventoryValidator());
    }

    public class UpdateProductInventoryValidator : AbstractValidator<ReqUpdateProductInventory>
    {
        public UpdateProductInventoryValidator()
        {
            RuleFor(x => x.ProductId)
             .NotNull().WithMessage("必填")
             .NotEmpty().WithMessage("必填");

            RuleFor(x => x.AdjustQuantity)
                .NotNull().WithMessage("必填")
                .NotEmpty().WithMessage("必填");

            RuleFor(x => x.Type)
                .NotNull().WithMessage("必填")
                .NotEmpty().WithMessage("必填");
        }
    }
}