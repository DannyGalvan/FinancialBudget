using FinancialBudget.Server.Entities.Request;
using FinancialBudget.Server.Validations.Common;
using FluentValidation;

namespace FinancialBudget.Server.Validations.BudgetItemValidators
{
    public class PartialBudgetItemValidator : PartialUpdateValidator<BudgetItemRequest, long?>
    {
        public PartialBudgetItemValidator()
        {
            When(x => x.Amount.HasValue, () =>
            {
                RuleFor(x => x.Amount)
                    .NotEmpty().WithMessage("El monto es requerido")
                    .GreaterThan(0).WithMessage("El monto debe ser mayor a cero");
            });
            When(x => x.BudgetId.HasValue, () =>
            {
                RuleFor(x => x.BudgetId)
                    .NotEmpty().WithMessage("El Id del presupuesto es requerido")
                    .GreaterThan(0).WithMessage("El Id del presupuesto no es valido");
            });
            When(x => x.OriginId.HasValue, () =>
            {
                RuleFor(x => x.OriginId)
                    .NotEmpty().WithMessage("El Id del origen es requerido")
                    .GreaterThan(0).WithMessage("El Id del origen no es valido");
            });
            When(x => x.SplitTypeId.HasValue, () =>
            {
                RuleFor(x => x.SplitTypeId)
                    .NotEmpty().WithMessage("El Id del tipo de división es requerido")
                    .GreaterThan(0).WithMessage("El Id del tipo de división no es valido");
            });
            When(x => x.RequestId.HasValue, () =>
            {
                RuleFor(x => x.RequestId)
                    .NotEmpty().WithMessage("El Id de la solicitud es requerido")
                    .GreaterThan(0).WithMessage("El Id de la solicitud no es valido");
            });
        }
    }
}
