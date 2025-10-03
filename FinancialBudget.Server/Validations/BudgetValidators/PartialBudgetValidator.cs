using FinancialBudget.Server.Entities.Request;
using FinancialBudget.Server.Validations.Common;
using FluentValidation;

namespace FinancialBudget.Server.Validations.BudgetValidators
{
    public class PartialBudgetValidator : PartialUpdateValidator<BudgetRequest, long?>
    {
        public PartialBudgetValidator()
        {
            When(x => x.AuthorizedAmount.HasValue, () =>
            {
                RuleFor(x => x.AuthorizedAmount)
                    .GreaterThan(0).WithMessage("El Monto Autorizado debe ser mayor a 0");
            });
            When(x => x.AvailableAmount.HasValue, () =>
            {
                RuleFor(x => x.AvailableAmount)
                    .GreaterThanOrEqualTo(0).WithMessage("El Monto Disponible debe ser mayor o igual a 0");
                When(x => x.AuthorizedAmount.HasValue, () =>
                {
                    RuleFor(x => x.AvailableAmount)
                        .Equal(x => x.AuthorizedAmount).WithMessage("El Monto Disponible debe ser igual al Monto Autorizado al crear el presupuesto");
                });
            });
            When(x => x.Period.HasValue, () =>
            {
                RuleFor(x => x.Period)
                    .GreaterThan(0).WithMessage("El Periodo debe ser mayor a 0");
            });
            When(x => x.State.HasValue, () =>
            {
                RuleFor(x => x.State)
                    .InclusiveBetween(0, 1).WithMessage("El Estado debe ser 0 (Inactivo) o 1 (Activo)");
            });
        }
    }
}
