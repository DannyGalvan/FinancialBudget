using FluentValidation.Results;
using FinancialBudget.Server.Entities.Request;
using FinancialBudget.Server.Entities.Response;

namespace FinancialBudget.Server.Services.Interfaces
{
    /// <summary>
    /// Defines the <see cref="IAuthService" />
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// The Auth
        /// </summary>
        /// <param name="model">The model<see cref="LoginRequest"/></param>
        /// <returns>The <see cref="Response{AuthResponse, List{ValidationFailure}}"/></returns>
        public Response<AuthResponse, List<ValidationFailure>> Auth(LoginRequest model);
    }
}
