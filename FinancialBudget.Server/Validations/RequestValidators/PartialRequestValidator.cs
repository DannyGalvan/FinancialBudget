using FinancialBudget.Server.Entities.Request;
using FinancialBudget.Server.Validations.Common;
using FluentValidation;

namespace FinancialBudget.Server.Validations.RequestValidators
{
    public class PartialRequestValidator : PartialUpdateValidator<RequestRequest, long?>
    {
        public PartialRequestValidator()
        {
            When(x => x.OriginId.HasValue, () =>
            {
                RuleFor(x => x.OriginId)
                    .NotNull().WithMessage("El Id del origen no puede ser nulo")
                    .NotEmpty().WithMessage("El Id del origen no puede ser vacio")
                    .GreaterThan(0).WithMessage("El Id del origen debe ser mayor a 0");
            });

            When(x => x.RequestAmount.HasValue, () =>
            {
                RuleFor(x => x.RequestAmount)
                    .NotNull().WithMessage("El monto solicitado no puede ser nulo")
                    .NotEmpty().WithMessage("El monto solicitado no puede ser vacio")
                    .GreaterThan(0).WithMessage("El monto solicitado debe ser mayor a 0");
            });
            When(x => x.Name is not null, () =>
            {
                RuleFor(x => x.Name)
                    .NotEmpty().WithMessage("El nombre referencia de la solicitud no puede ser vacio")
                    .NotNull().WithMessage("El nombre referencia de la solicitud no puede ser nulo");
            });
            When(x => x.Reason is not null, () =>
            {
                RuleFor(x => x.Reason)
                    .NotEmpty().WithMessage("La razón de la solicitud no puede ser vacio")
                    .NotNull().WithMessage("La razón de la solicitud no puede ser nulo");
            });
            When(x => x.RequestDate is not null, () =>
            {
                RuleFor(x => x.RequestDate)
                    .NotEmpty().WithMessage("La fecha de la solicitud no puede ser vacio")
                    .NotNull().WithMessage("La fecha de la solicitud no puede ser nulo");
            });
            When(x => x.RequestStatusId != 0, () =>
            {
                RuleFor(x => x.RequestStatusId)
                    .GreaterThan(0).WithMessage("El Id del estado de la solicitud debe ser mayor a 0");
            });
            When(x => x.Email is not null, () =>
            {
                RuleFor(x => x.Email)
                    .NotEmpty().WithMessage("El correo electrónico no puede ser vacío")
                    .NotNull().WithMessage("El correo electrónico no puede ser nulo")
                    .EmailAddress().WithMessage("El correo electrónico no es válido");
            });
            When(x => x.PriorityId.HasValue, () =>
            {
                RuleFor(x => x.PriorityId)
                    .NotNull().WithMessage("El Id de la prioridad no puede ser nulo")
                    .NotEmpty().WithMessage("El Id de la prioridad no puede ser vacio")
                    .GreaterThan(0).WithMessage("El Id de la prioridad debe ser mayor a 0");
            });
            When(x => x.State.HasValue, () =>
            {
                RuleFor(x => x.State)
                    .NotNull().WithMessage("El estado no puede ser nulo")
                    .NotEmpty().WithMessage("El estado no puede ser vacio")
                    .GreaterThanOrEqualTo(0).WithMessage("El estado debe ser mayor o igual a 0");
            });
        }
    }
}
