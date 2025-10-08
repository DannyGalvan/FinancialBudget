using NpgsqlTypes;
using Serilog.Sinks.PostgreSQL;

namespace FinancialBudget.Server.Configs.Extensions
{
    using Serilog;
    using Serilog.Extensions.Logging;

    /// <summary>
    /// Defines the <see cref="LogPersistConfiguration" />
    /// </summary>
    public static class LogPersistConfiguration
    {
        /// <summary>
        /// The AddLoggerConfiguration
        /// </summary>
        /// <param name="services">The services<see cref="IServiceCollection"/></param>
        /// <param name="configuration">The configuration<see cref="IConfigurationRoot"/></param>
        /// <returns>The <see cref="IServiceCollection"/></returns>
        public static IServiceCollection AddLoggerConfiguration(this IServiceCollection services, IConfigurationRoot configuration)
        {
            services.AddLogging(loggerBuilder =>
            {
                IDictionary<string, ColumnWriterBase> columnWriters = new Dictionary<string, ColumnWriterBase>
                {
                    { "message", new RenderedMessageColumnWriter(NpgsqlDbType.Text) },
                    { "message_template", new MessageTemplateColumnWriter(NpgsqlDbType.Text) },
                    { "level", new LevelColumnWriter(true, NpgsqlDbType.Varchar) },
                    { "raise_date", new TimestampColumnWriter(NpgsqlDbType.TimestampTz) },
                    { "exception", new ExceptionColumnWriter(NpgsqlDbType.Text) },
                    { "properties", new LogEventSerializedColumnWriter(NpgsqlDbType.Jsonb) },
                    { "props_test", new PropertiesColumnWriter(NpgsqlDbType.Jsonb) },
                    { "machine_name", new SinglePropertyColumnWriter("MachineName", PropertyWriteMethod.ToString, NpgsqlDbType.Text, "l") }
                };

                loggerBuilder.AddConfiguration(configuration.GetSection("Serilog"));

                var log = new LoggerConfiguration()
                    .MinimumLevel.Information()
                    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
                    .Enrich.FromLogContext()
                    .WriteTo.PostgreSQL(
                        connectionString: configuration.GetConnectionString("Default"),
                        tableName: "Logs",
                        columnOptions: columnWriters,
                        needAutoCreateTable: true,  
                        restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Warning)
                    .CreateLogger();

                loggerBuilder.AddProvider(new SerilogLoggerProvider(log));
            });

            return services;
        }
    }
}
