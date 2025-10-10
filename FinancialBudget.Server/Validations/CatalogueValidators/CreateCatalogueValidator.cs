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
        }
    }
}
