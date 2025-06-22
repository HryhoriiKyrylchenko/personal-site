namespace PersonalSite.Web.Logging;

public class IdColumnWriter : ColumnWriterBase
{
    public IdColumnWriter() : base(dbType: NpgsqlTypes.NpgsqlDbType.Uuid) { }

    public override object GetValue(LogEvent logEvent, IFormatProvider? formatProvider = null)
    {
        return Guid.NewGuid();
    }
}