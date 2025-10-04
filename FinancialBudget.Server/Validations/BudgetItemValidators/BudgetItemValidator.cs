using FluentValidation;
using FinancialBudget.Server.Entities.Request;

namespace FinancialBudget.Server.Validations.BudgetItemValidators
{
    public class BudgetItemValidator : AbstractValidator<BudgetItemRequest>
    {
        public BudgetItemValidator()
        {
            RuleFor(x => x.BudgetId).GreaterThan(0).WithMessage("BudgetId es obligatorio");
            RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Amount debe ser mayor que cero");
            RuleFor(x => x.OriginId).GreaterThan(0).WithMessage("OriginId es obligatorio");
            RuleFor(x => x.SplitTypeId).GreaterThan(0).WithMessage("SplitTypeId es obligatorio");
            RuleFor(x => x.RequestId).GreaterThan(0).WithMessage("RequestId es obligatorio");
        }
    }
}
