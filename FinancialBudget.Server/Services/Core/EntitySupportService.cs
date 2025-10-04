using System.Reflection;
using FinancialBudget.Server.Interceptors.Interfaces;
using FinancialBudget.Server.Services.Interfaces;
using FinancialBudget.Server.Utils;
using FluentValidation;
using Lombok.NET;

namespace FinancialBudget.Server.Services.Core
{
    [AllArgsConstructor]
    public partial class EntitySupportService : IEntitySupportService
    {
        private readonly IServiceProvider _serviceProvider;

        public IValidator<TRequest> GetValidator<TRequest>(string key)
        {
            return _serviceProvider.GetRequiredKeyedService<IValidator<TRequest>>(key);
        }

        public IEnumerable<IEntityBeforeCreateInterceptor<TEntity, TRequest>> GetBeforeCreateInterceptors<TEntity, TRequest>()
        {
            return _serviceProvider
                .GetServices<IEntityBeforeCreateInterceptor<TEntity, TRequest>>()
                .OrderBy(i => i.GetType().GetCustomAttribute<OrderAttribute>()?.Priority ?? int.MaxValue);
        }

        public IEnumerable<IEntityBeforeUpdateInterceptor<TEntity, TRequest>> GetBeforeUpdateInterceptors<TEntity, TRequest>()
        {
            return _serviceProvider
                .GetServices<IEntityBeforeUpdateInterceptor<TEntity, TRequest>>()
                .OrderBy(i => i.GetType().GetCustomAttribute<OrderAttribute>()?.Priority ?? int.MaxValue);
        }

        public IEnumerable<IEntityAfterCreateInterceptor<TEntity, TRequest>> GetAfterCreateInterceptors<TEntity, TRequest>()
        {
            return _serviceProvider
                .GetServices<IEntityAfterCreateInterceptor<TEntity, TRequest>>()
                .OrderBy(i => i.GetType().GetCustomAttribute<OrderAttribute>()?.Priority ?? int.MaxValue);
        }

        public IEnumerable<IEntityAfterUpdateInterceptor<TEntity, TRequest>> GetAfterUpdateInterceptors<TEntity, TRequest>()
        {
            return _serviceProvider
                .GetServices<IEntityAfterUpdateInterceptor<TEntity, TRequest>>()
                .OrderBy(i => i.GetType().GetCustomAttribute<OrderAttribute>()?.Priority ?? int.MaxValue);
        }

        public IEnumerable<IEntityAfterPartialUpdateInterceptor<TEntity, TRequest>> GetAfterPartialUpdateInterceptors<TEntity, TRequest>()
        {
            return _serviceProvider
                .GetServices<IEntityAfterPartialUpdateInterceptor<TEntity, TRequest>>()
                .OrderBy(i => i.GetType().GetCustomAttribute<OrderAttribute>()?.Priority ?? int.MaxValue);
        }
    }

}