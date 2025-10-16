using FinancialBudget.Server.Entities.Response;
using FluentValidation.Results;

namespace FinancialBudget.Server.Interceptors.Interfaces
{
    /// <summary>
    /// Defines the <see>
    ///     <cref>IEntityAfterUpdateInterceptor{T, in TRequest}</cref>
    /// </see>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TRequest"></typeparam>
    public interface IEntityAfterUpdateInterceptor<T, in TRequest>
    {
        /// <summary>
        /// The Execute
        /// </summary>
        /// <param name="response">The response<see>
        ///         <cref>Response{T, List{ValidationFailure}}</cref>
        ///     </see>
        /// </param>
        /// <param name="request">The request<see cref="TRequest"/></param>
        /// <param name="prevState">The prevState<see cref="T"/></param>
        /// <returns>The <see>
        ///         <cref>Response{T, List{ValidationFailure}}</cref>
        ///     </see>
        /// </returns>
        Response<T, List<ValidationFailure>> Execute(Response<T, List<ValidationFailure>> response, TRequest request, T prevState);
    }
}
