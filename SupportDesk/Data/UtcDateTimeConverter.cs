using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace SupportDesk.Data;


    public sealed class UtcDateTimeConverter()
        : ValueConverter<DateTime, DateTime>(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
