using FinancialBudget.Server.Entities.Request;
using FinancialBudget.Server.Validations.Common;
using FluentValidation;

namespace FinancialBudget.Server.Validations.RequestValidators
{
    public class CreateRequestValidations : CreateValidator<RequestRequest, long?>
    {
        public CreateRequestValidations()
        {
            RuleFor(x => x.OriginId)
                .NotNull().WithMessage("El Id del origen no puede ser nulo")
                .NotEmpty().WithMessage("El Id del origen no puede ser vacio")
                .GreaterThan(0).WithMessage("El Id del origen debe ser mayor a 0");

            RuleFor(x => x.RequestAmount)
                .NotNull().WithMessage("El monto solicitado no puede ser nulo")
                .NotEmpty().WithMessage("El monto solicitado no puede ser vacio")
                .GreaterThan(0).WithMessage("El monto solicitado debe ser mayor a 0");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre referencia de la solicitud no puede ser vacio")
                .NotNull().WithMessage("El nombre referencia de la solicitud no puede ser nulo");

            RuleFor(x => x.Reason)
                .NotEmpty().WithMessage("La razón de la solicitud no puede ser vacio")
                .NotNull().WithMessage("La razón de la solicitud no puede ser nulo");

            RuleFor(x => x.RequestDate)
                .NotEmpty().WithMessage("La fecha de la solicitud no puede ser vacio")
                .NotNull().WithMessage("La fecha de la solicitud no puede ser nulo");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("El correo electrónico no puede ser vacío")
                .NotNull().WithMessage("El correo electrónico no puede ser nulo")
                .EmailAddress().WithMessage("El correo electrónico no es válido");

            RuleFor(x => x.State)
                .Null().WithMessage("El Estado no debes mandarlo al crear una solicitud");

            RuleFor(x => x.PriorityId)
                .NotNull().WithMessage("El Id de la prioridad no puede ser nulo")
                .NotEmpty().WithMessage("El Id de la prioridad no puede ser vacio")
                .GreaterThan(0).WithMessage("El Id de la prioridad debe ser mayor a 0");

        }
    }
}
