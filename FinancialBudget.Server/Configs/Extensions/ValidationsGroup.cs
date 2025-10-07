using FinancialBudget.Server.Validations.BudgetItemValidators;
using FinancialBudget.Server.Validations.BudgetValidators;
using FinancialBudget.Server.Validations.RequestValidators;

namespace FinancialBudget.Server.Configs.Extensions
{
    using FinancialBudget.Server.Entities.Request;
    using FinancialBudget.Server.Validations.Auth;
    using FinancialBudget.Server.Validations.CatalogueValidators;
    using FluentValidation;

    /// <summary>
    /// Defines the <see cref="ValidationsGroup" />
    /// </summary>
    public static class ValidationsGroup
    {
        /// <summary>
        /// The AddValidationsGroup
        /// </summary>
        /// <param name="services">The services<see cref="IServiceCollection"/></param>
        /// <returns>The <see cref="IServiceCollection"/></returns>
        public static IServiceCollection AddValidationsGroup(this IServiceCollection services)
        {
            //auth validations
            services.AddScoped<IValidator<LoginRequest>, LoginValidations>();
            services.AddScoped<IValidator<ChangePasswordRequest>, ChangePasswordValidations>();
            services.AddScoped<IValidator<ResetPasswordRequest>, ResetPasswordValidations>();
            services.AddScoped<IValidator<RecoveryPasswordRequest>, RecoveryPasswordValidations>();
            services.AddScoped<IValidator<RegisterRequest>, RegisterValidations>();

            //catalogue validations
            services.AddKeyedScoped<IValidator<CatalogueRequest>, CreateCatalogueValidator>("Create");
            services.AddKeyedScoped<IValidator<CatalogueRequest>, UpdateCatalogueValidator>("Update");
            services.AddKeyedScoped<IValidator<CatalogueRequest>, PartialCatalogueValidator>("Partial");

            //budget validations
            services.AddKeyedScoped<IValidator<BudgetRequest>, CreateBudgetValidator>("Create");
            services.AddKeyedScoped<IValidator<BudgetRequest>, UpdateBudgetValidator>("Update");
            services.AddKeyedScoped<IValidator<BudgetRequest>, PartialBudgetValidator>("Partial");

            //budget item validations
            services.AddKeyedScoped<IValidator<BudgetItemRequest>, CreateBudgetItemValidator>("Create");
            services.AddKeyedScoped<IValidator<BudgetItemRequest>, UpdateBudgetItemValidator>("Update");
            services.AddKeyedScoped<IValidator<BudgetItemRequest>, PartialBudgetItemValidator>("Partial");

            //request validations
            services.AddKeyedScoped<IValidator<RequestRequest>, CreateRequestValidations>("Create");
            services.AddKeyedScoped<IValidator<RequestRequest>, UpdateRequestValidation>("Update");
            services.AddKeyedScoped<IValidator<RequestRequest>, PartialRequestValidator>("Partial");

            return services;
        }

    }

}
