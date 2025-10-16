namespace FinancialBudget.Server.Configs.Extensions
{
    using Microsoft.OpenApi.Models;
    using System.Reflection;

    /// <summary>
    /// Defines the <see cref="SwaggerConfiguration" />
    /// </summary>
    public static class SwaggerConfiguration
    {
        /// <summary>
        /// The AddSwaggerConfiguration
        /// </summary>
        /// <param name="services">The services<see cref="IServiceCollection"/></param>
        public static void AddSwaggerConfiguration(this IServiceCollection services)
        {
            //Add the Swagger configuration
            services.AddEndpointsApiExplorer();

            //Add the Swagger configuration
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Modulo Finanzas API",
                    Description = "An ASP.NET Core Web API for managing financial operations",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Grupo Financiero",
                        Url = new Uri("https://example.com/contact")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Example License",
                        Url = new Uri("https://example.com/license")
                    }
                });

                options.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Description = "Encabezado de autorización JWT utilizando el esquema Portador."
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "bearerAuth" }
                        },
                        Array.Empty<string>()
                    }
                });

                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });
        }
    }
}
