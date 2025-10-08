using FinancialBudget.Server.Context;
using FinancialBudget.Server.Entities.Models;
using FinancialBudget.Server.Entities.Response;
using FinancialBudget.Server.Services.Interfaces;
using Lombok.NET;
using Microsoft.EntityFrameworkCore;

namespace FinancialBudget.Server.Services.Business
{
    [AllArgsConstructor]
    public partial class AuthorizationRequestFlow : IAuthorizationRequestFlow
    {
        private readonly DataContext _db;
        private readonly ISendMail _sendMail;

        public Response<string> ProcessAuthorization(Request request, long state, string comment)
        {
            var response = new Response<string>();

            var current = _db.Traceabilities.AsNoTracking().FirstOrDefault(x => x.RequestId == request.Id && x.State == 1);

            response.Success = false;
            response.Message = "No se encontró una traza activa para esta solicitud.";

            if (current == null) return response;

            var hasDisponibility = _db.Budgets.FirstOrDefault(x => x.Period == DateTime.Now.Year && x.AvailableAmount >= request.RequestAmount);

            if (state == 2 && hasDisponibility == null)
            {
                response.Success = false;
                response.Message = "No hay disponibilidad presupuestal para aprobar esta solicitud.";
                return response;
            }

            current.State = 0;
            current.UpdatedAt = DateTimeOffset.UtcNow;

            var newTrace = new Traceability
            {
                RequestId = request.Id,
                State = 1,
                CreatedAt = DateTimeOffset.UtcNow,
                Comments = comment,
                CreatedBy = current.CreateUserId,
                CreateUserId = current.CreateUserId,
                AuthorizeUserId = current.AuthorizeUserId
            };

            switch (state)
            {
                case 3: // Rechazada
                    newTrace.RequestStatusId = 3;
                    newTrace.AuthorizeUserId = current.AuthorizeUserId;
                    newTrace.State = 0; // Ya no será trazable
                    break;
                case 2:
                    newTrace.RequestStatusId = 2; // Aprobada
                    newTrace.AuthorizeUserId = current.AuthorizeUserId;
                    newTrace.State = 1;

                    var newBudgetItem = new BudgetItem
                    {
                        RequestId = request.Id,
                        Amount = request.RequestAmount,
                        CreatedAt = DateTimeOffset.UtcNow,
                        BudgetId = _db.Budgets.FirstOrDefault(x => x.Period == DateTime.Now.Year)?.Id ?? 0,
                        OriginId = request.OriginId,
                        SplitTypeId = 1,
                        State = 1,
                        UpdatedAt = null,
                        CreatedBy = current.CreateUserId,
                        UpdatedBy = null,
                    };

                    _db.Entry(newBudgetItem).State = EntityState.Added;
                    _db.BudgetItems.Add(newBudgetItem);

                    // Actualizar la disponibilidad del presupuesto
                    if (hasDisponibility != null)
                    {
                        hasDisponibility.AvailableAmount -= request.RequestAmount;
                        _db.Entry(hasDisponibility).State = EntityState.Modified;
                        _db.Budgets.Update(hasDisponibility);
                    }

                    _db.SaveChanges();

                    break;
                default:
                    response.Success = false;
                    response.Message = "Estado no válido para la autorización.";
                    return response;
            }

            _db.Entry(current).State = EntityState.Modified;
            _db.Entry(newTrace).State = EntityState.Added;
            _db.Traceabilities.Update(current);
            _db.Traceabilities.Add(newTrace);
            _db.SaveChanges();

            // Enviar correo electrónico

            var emailSubject = state == 2 ? "Solicitud Aprobada" : "Solicitud Rechazada";

            var emailBody = state == 2
                ? $"Su solicitud con ID {request.Id} ha sido aprobada.\n\nComentarios: {comment}"
                : $"Su solicitud con ID {request.Id} ha sido rechazada.\n\nComentarios: {comment}";

            _sendMail.Send(request.Email, emailSubject, emailBody);

            response.Success = true;
            response.Message = "La solicitud ha sido procesada exitosamente.";
            response.Data = emailBody;


            return response;
        }
    }
}
