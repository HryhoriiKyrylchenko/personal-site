namespace PersonalSite.Web.Configuration;

public class SerilogConfigurator
{
    public static void Configure(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        var columnWriters = new Dictionary<string, ColumnWriterBase>
        {
            { "Id", new IdColumnWriter() },
            { "Timestamp", new TimestampColumnWriter() },
            { "Level", new LevelColumnWriter() },
            { "Message", new RenderedMessageColumnWriter() },
            { "MessageTemplate", new MessageTemplateColumnWriter() },
            { "Exception", new ExceptionColumnWriter() },
            { "Properties", new PropertiesColumnWriter() },
            { "SourceContext", new SinglePropertyColumnWriter("SourceContext") }
        };

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .Enrich.FromLogContext()
            .WriteTo.File(
                "Logs/log-.txt",
                rollingInterval: RollingInterval.Day,
                restrictedToMinimumLevel: LogEventLevel.Information
            )
            .WriteTo.PostgreSQL(
                connectionString: connectionString,
                tableName: "Logs",
                columnOptions: columnWriters,
                needAutoCreateTable: false,
                restrictedToMinimumLevel: LogEventLevel.Error
            )
            .CreateLogger();
    }
}