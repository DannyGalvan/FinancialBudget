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
            services.AddDbContext<DataContext>((sp, options) =>
            {
                var env = sp.GetRequiredService<IHostEnvironment>();

                options.UseNpgsql(configuration.GetConnectionString("default"));

                if (env.IsDevelopment())
                {
                    options
                        .EnableDetailedErrors()
                        .EnableSensitiveDataLogging();
                }
                // En Producción (no Development) NO se activan estas opciones.
            });

            return services;
        }
    }
}
