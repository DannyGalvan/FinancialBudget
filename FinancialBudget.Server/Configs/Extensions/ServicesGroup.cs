using FinancialBudget.Server.Entities.Models;
using FinancialBudget.Server.Entities.Request;
using FinancialBudget.Server.Filters;
using FinancialBudget.Server.Interceptors.Interfaces;
using FinancialBudget.Server.Interceptors.RequestInterceptors;
using FinancialBudget.Server.Services.Business;
using FinancialBudget.Server.Services.Core;
using FinancialBudget.Server.Services.Interfaces;
using FinancialBudget.Server.Utils;
using Microsoft.AspNetCore.Authorization;

namespace FinancialBudget.Server.Configs.Extensions
{

    /// <summary>
    /// Defines the <see cref="ServicesGroup" />
    /// </summary>
    public static class ServicesGroup
    {
        /// <summary>
        /// The AddServiceGroup
        /// </summary>
        /// <param name="services">The services<see cref="IServiceCollection"/></param>
        /// <returns>The <see cref="IServiceCollection"/></returns>
        public static IServiceCollection AddServiceGroup(this IServiceCollection services)
        {
            // entities services
            services.AddScoped<IAuthService, AuthService>();

            services
                .AddScoped<IEntityService<Budget,BudgetRequest,long>, EntityService<Budget,BudgetRequest,long>>();
            services 
                .AddScoped<IEntityService<BudgetItem, BudgetItemRequest, long>, EntityService<BudgetItem, BudgetItemRequest, long>>();
            services
                .AddScoped<IEntityService<Request, RequestRequest, long>, EntityService<Request, RequestRequest, long>>();
            services.AddScoped<IAuthorizationRequestFlow, AuthorizationRequestFlow>();

            // catalogue services
            services
                .AddKeyedScoped<IEntityService<Origin, CatalogueRequest, long>,
                    EntityService<Origin, CatalogueRequest, long>>("Origin");
            services
                .AddKeyedScoped<IEntityService<Priority, CatalogueRequest, long>,
                    EntityService<Priority, CatalogueRequest, long>>("Priority");
            services
                .AddKeyedScoped<IEntityService<SplitType, CatalogueRequest, long>,
                    EntityService<SplitType, CatalogueRequest, long>>("SplitType");
            services
                .AddKeyedScoped<IEntityService<RequestStatus, CatalogueRequest, long>,
                    EntityService<RequestStatus, CatalogueRequest, long>>("RequestStatus");

            // util services
            services.AddScoped<IEntitySupportService, EntitySupportService>();
            services.AddScoped<IFilterTranslator, FilterTranslator>();

            // interceptors
            services.AddScoped<IEntityAfterCreateInterceptor<Request, RequestRequest>, InitialTraceabilityAndSendMail>();
            services.AddScoped<IEntityAfterUpdateInterceptor<Request, RequestRequest>, UpdateFlowTraceabilityAndSendMail>();
            services.AddScoped<IEntityAfterPartialUpdateInterceptor<Request, RequestRequest>, PartialFlowTraceabilityAndSendMail>();

            // other services
            services.AddScoped<ISendMail, SendEmail>();
            services.AddSingleton<IAuthorizationHandler, MultipleClaimsHandler>();


            return services;
        }
    }
}
