using FinancialBudget.Server.Entities.Request;
using FinancialBudget.Server.Utils;
using FinancialBudget.Server.Validations.Common;
using FluentValidation;

namespace FinancialBudget.Server.Validations.BudgetItemValidators
{
    public class CreateBudgetItemValidator : CreateValidator<BudgetItemRequest, long?>
    {
        public CreateBudgetItemValidator()
        {
            RuleFor(x => x.Amount)
                .NotEmpty().WithMessage("El monto es requerido")
                .GreaterThan(0).WithMessage("El monto debe ser mayor a cero");
            RuleFor(x => x.BudgetId)
                .NotEmpty().WithMessage("El Id del presupuesto es requerido")
                .Must(Util.HasValidId).WithMessage("El Id del presupuesto no es valido");
            RuleFor(x => x.OriginId)
                .NotEmpty().WithMessage("El Id del origen es requerido")
                .Must(Util.HasValidId).WithMessage("El Id del origen no es valido");
            RuleFor(x => x.SplitTypeId)
                .NotEmpty().WithMessage("El Id del tipo de división es requerido")
                .Must(Util.HasValidId).WithMessage("El Id del tipo de división no es valido");
            RuleFor(x => x.RequestId)
                .NotEmpty().WithMessage("El Id de la solicitud es requerido")
                .Must(Util.HasValidId).WithMessage("El Id de la solicitud no es valido");

        }
    }
}
  