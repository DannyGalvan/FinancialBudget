using FinancialBudget.Server.Services.Interfaces;
using AuthService = FinancialBudget.Server.Services.Core.AuthService;
using SendEmail = FinancialBudget.Server.Services.Core.SendEmail;


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

            // other services
            services.AddScoped<ISendMail, SendEmail>();

            return services;
        }
    }
}
