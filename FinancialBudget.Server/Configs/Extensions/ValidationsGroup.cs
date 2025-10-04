namespace FinancialBudget.Server.Configs.Extensions
{
    using FinancialBudget.Server.Entities.Request;
    using FinancialBudget.Server.Validations.Auth;
    using FinancialBudget.Server.Validations.BudgetItemValidators;
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

<<<<<<< Updated upstream
=======
            //budget validations
            services.AddKeyedScoped<IValidator<BudgetRequest>, CreateBudgetValidator>("Create");
            services.AddKeyedScoped<IValidator<BudgetRequest>, UpdateBudgetValidator>("Update");
            services.AddKeyedScoped<IValidator<BudgetRequest>, PartialBudgetValidator>("Partial");

            //budgetItem
            // BudgetItem validations
            services.AddKeyedScoped<IValidator<BudgetItemRequest>, BudgetItemValidator>("Create");
            services.AddKeyedScoped<IValidator<BudgetItemRequest>, BudgetItemValidator>("Update");
            services.AddKeyedScoped<IValidator<BudgetItemRequest>, BudgetItemValidator>("Partial");



>>>>>>> Stashed changes
            return services;
        }

    }

}
