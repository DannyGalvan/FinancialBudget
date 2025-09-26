using FinancialBudget.Server.Mappers;
using Mapster;
using MapsterMapper;

namespace FinancialBudget.Server.Configs.Extensions
{
    public static class MapsterSettings
    {
        public static IServiceCollection AddMapsterSettings(this IServiceCollection services)
        {

            MapsterConfig.RegisterMappings();
            services.AddSingleton(TypeAdapterConfig.GlobalSettings);
            services.AddScoped<IMapper, ServiceMapper>();

            return services;
        }
    }
}
