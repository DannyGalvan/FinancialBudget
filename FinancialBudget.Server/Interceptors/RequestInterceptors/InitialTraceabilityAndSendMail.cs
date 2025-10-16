using FinancialBudget.Server.Context;
using FinancialBudget.Server.Entities.Models;
using FinancialBudget.Server.Entities.Request;
using FinancialBudget.Server.Entities.Response;
using FinancialBudget.Server.Interceptors.Interfaces;
using FinancialBudget.Server.Services.Core;
using FinancialBudget.Server.Services.Interfaces;
using FinancialBudget.Server.Utils;
using FluentValidation.Results;
using Lombok.NET;

namespace FinancialBudget.Server.Interceptors.RequestInterceptors
{
    [Order(1)]
    [AllArgsConstructor]
    public partial class InitialTraceabilityAndSendMail : IEntityAfterCreateInterceptor<Request,RequestRequest>
    {
        private readonly ILogger<EntityService<Request, RequestRequest, long>> _logger;
        private readonly ISendMail _sendMail;
        private readonly DataContext _db;

        public Response<Request, List<ValidationFailure>> Execute(Response<Request, List<ValidationFailure>> response, RequestRequest request)
        {
            try
            {
                var userAutorize = _db.Users.FirstOrDefault(u => u.RolId == 1 && u.State == 1);

                if (userAutorize == null)
                {
                    response.Success = false;
                    response.Message = "No se encontró un usuario autorizador activo.";
                    _logger.LogError("No se encontró un usuario autorizador activo para la solicitud {order}, Usuario: {user}", response.Data!.Id, response.Data.CreatedBy);
                    return response;
                }

                Traceability trace = new Traceability
                {
                    RequestId = response.Data!.Id,
                    RequestStatusId = 1, // Enviado
                    State = 1, // Activo
                    CreatedAt = DateTimeOffset.UtcNow,
                    CreateUserId = response.Data.CreatedBy,
                    AuthorizeUserId = userAutorize.Id, 
                    Comments = "Solicitud creada y en espera de autorización."
                };

                _db.Traceabilities.Add(trace);
                _db.SaveChanges();

                _sendMail.Send(request.Email!, "Solicitud Recibida", $"Su solicitud con ID {response.Data.Id} ha sido recibida y está en espera de autorización.");

                _sendMail.Send(userAutorize.Email, "Solicitud Recibida", $"La solicitud con ID {response.Data.Id} ha sido recibida y está en espera de autorización.");

                response.Success = true;
                response.Message = "Solicitud creada exitosamente con trazabilidad inicial y correo enviado.";

                return response;
            }
            catch (Exception e)
            {
                response.Success = false;
                response.Message = "Error al iniciar proceso de autoriizacion despues de crear la solicitud en la bd";

                _logger.LogError(e, "Error al iniciar proceso de autoriizacion despues de crear la solicitud en la bd {order}, Usuario: {user} Error: {error}", response.Data!.Id, response.Data.CreatedBy, e.Message);

                return response;
            }
        }
    }
}
