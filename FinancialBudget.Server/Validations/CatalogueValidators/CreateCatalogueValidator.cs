using FinancialBudget.Server.Entities.Request;
using FinancialBudget.Server.Validations.Common;
using FluentValidation;

namespace FinancialBudget.Server.Validations.CatalogueValidators
{
    public class CreateCatalogueValidator : CreateValidator<CatalogueRequest, string?>
    {
        public CreateCatalogueValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre del país es obligatorio.")
                .MaximumLength(100).WithMessage("El nombre del país no puede exceder los 100 caracteres.");
            RuleFor(x => x.State)
                .InclusiveBetween(0, 1).WithMessage("El estado del país debe ser 0 (inactivo) o 1 (activo).");
        }
    }
}
