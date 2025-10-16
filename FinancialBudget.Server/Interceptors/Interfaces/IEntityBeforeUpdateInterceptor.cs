using FinancialBudget.Server.Entities.Response;
using FluentValidation.Results;

namespace FinancialBudget.Server.Interceptors.Interfaces
{
    /// <summary>
    /// Defines the <see>
    ///     <cref>IEntityBeforeUpdateInterceptor{T, in TRequest}</cref>
    /// </see>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TRequest"></typeparam>
    public interface IEntityBeforeUpdateInterceptor<T, in TRequest>
    {
        /// <summary>
        /// The Execute
        /// </summary>
        /// <param name="response">The response<see>
        ///         <cref>Response{T, List{ValidationFailure}}</cref>
        ///     </see>
        /// </param>
        /// <param name="request">The request<see cref="TRequest"/></param>
        /// <returns>The <see>
        ///         <cref>Response{T, List{ValidationFailure}}</cref>
        ///     </see>
        /// </returns>
        Response<T, List<ValidationFailure>> Execute(Response<T, List<ValidationFailure>> response, TRequest request);
    }
}
