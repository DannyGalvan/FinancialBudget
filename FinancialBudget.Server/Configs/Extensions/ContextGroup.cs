namespace FinancialBudget.Server.Configs.Extensions
{
    using FinancialBudget.Server.Context;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Defines the <see cref="ContextGroup" />
    /// </summary>
    public static class ContextGroup
    {
        /// <summary>
        /// The AddContextGroup
        /// </summary>
        /// <param name="services">The services<see cref="IServiceCollection"/></param>
        /// <param name="configuration">The configuration<see cref="IConfiguration"/></param>
        /// <returns>The <see cref="IServiceCollection"/></returns>
        public static IServiceCollection AddContextGroup(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DataContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("default"))
                    .EnableDetailedErrors()
                    .EnableSensitiveDataLogging();
            });

            return services;
        }
    }
}
