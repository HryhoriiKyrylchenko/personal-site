using NpgsqlTypes;

namespace PersonalSite.Web.Logging;

public class IdColumnWriter(NpgsqlDbType dbType = NpgsqlDbType.Uuid) : ColumnWriterBase(dbType)
{
    public override object GetValue(LogEvent logEvent, IFormatProvider? formatProvider = null)
    {
        return (object) Guid.NewGuid();
    }
}