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
    public partial class PartialFlowTraceabilityAndSendMail : IEntityAfterPartialUpdateInterceptor<Request, RequestRequest>
    {
        private readonly ILogger<EntityService<Request, RequestRequest, long>> _logger;
        private readonly IAuthorizationRequestFlow _authorizationFlowService;
        public Response<Request, List<ValidationFailure>> Execute(Response<Request, List<ValidationFailure>> response, RequestRequest request, Request prevState)
        {
            try
            {
                if (request.RequestStatusId == prevState.RequestStatusId || request.RequestStatusId == prevState.RequestStatusId)
                    return response;

                var result = _authorizationFlowService.ProcessAuthorization(response.Data!, request.RequestStatusId!.Value, request.Comments ?? string.Empty);

                if (result.Success) return response;

                response.Success = false;
                response.Message = result.Message;
                _logger.LogError("Error al procesar autorizacion de la solicitud {order}, Usuario: {user} Error: {error}", response.Data!.Id, response.Data.CreatedBy, result.Message);

                return response;
            }
            catch (Exception e)
            {
                response.Success = false;
                response.Message = "Error al realizar proceso de autorizacion luego de actualizar la solicitud en la bd";

                _logger.LogError(e, "Error al realizar proceso de autorizacion luego de actualizar la solicitud {order}, Usuario: {user} Error: {error}", response.Data!.Id, response.Data.CreatedBy, e.Message);

                return response;
            }
        }
    }
}
