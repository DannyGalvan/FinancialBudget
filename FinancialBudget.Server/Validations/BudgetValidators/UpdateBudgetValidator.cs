using FinancialBudget.Server.Entities.Request;
using FinancialBudget.Server.Validations.Common;
using FluentValidation;

namespace FinancialBudget.Server.Validations.BudgetValidators
{
    public class UpdateBudgetValidator : UpdateValidator<BudgetRequest, long?>
    {
        public UpdateBudgetValidator()
        {
            RuleFor(x => x.AuthorizedAmount)
                .NotNull().WithMessage("El Monto Autorizado no puede ser nulo")
                .GreaterThan(0).WithMessage("El Monto Autorizado debe ser mayor a 0");

            RuleFor(x => x.AvailableAmount)
                .NotNull().WithMessage("El Monto Disponible no puede ser nulo")
                .GreaterThanOrEqualTo(0).WithMessage("El Monto Disponible debe ser mayor o igual a 0");

            RuleFor(x => x.AvailableAmount)
                .Equal(x => x.AuthorizedAmount).WithMessage("El Monto Disponible debe ser igual al Monto Autorizado al crear el presupuesto");

            RuleFor(x => x.Period)
                .NotNull().WithMessage("El Periodo no puede ser nulo")
                .GreaterThan(0).WithMessage("El Periodo debe ser mayor a 0");
        }
    }
}
