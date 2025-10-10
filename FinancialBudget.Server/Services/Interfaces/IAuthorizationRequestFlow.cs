using FinancialBudget.Server.Entities.Models;
using FinancialBudget.Server.Entities.Response;

namespace FinancialBudget.Server.Services.Interfaces
{
    public interface IAuthorizationRequestFlow
    {
        Response<string> ProcessAuthorization(Request request, long state, string comment);
    }
}
