namespace MarketPOS.Shared.DataConventer;
public class JsonDateConverter : JsonConverter<DateTime>
{
    /// <summary>
    /// Represents the date and time format string used for formatting and parsing operations.
    /// </summary>
    /// <remarks>The format string follows the standard .NET date and time format conventions.  It specifies a
    /// pattern of "yyyy-MM-dd HH:mm:ss tt", which includes a four-digit year,  two-digit month, two-digit day, 24-hour
    /// clock hour, minutes, seconds, and an AM/PM designator.</remarks>

    private const string Format = "yyyy-MM-dd HH:mm:ss tt";

    /// <summary>
    /// Reads and converts the JSON string representation of a date and time to a <see cref="DateTime"/> object.
    /// </summary>
    /// <param name="reader">The <see cref="Utf8JsonReader"/> to read the JSON data from.</param>
    /// <param name="typeToConvert">The type of the object to convert. This parameter is ignored in this implementation.</param>
    /// <param name="options">The <see cref="JsonSerializerOptions"/> to use during deserialization. This parameter is ignored in this
    /// implementation.</param>
    /// <returns>A <see cref="DateTime"/> object parsed from the JSON string.</returns>
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return DateTime.Parse(reader.GetString()!);
    }

    /// <summary>
    /// Writes the specified <see cref="DateTime"/> value as a JSON string using the specified format.
    /// </summary>
    /// <remarks>The <see cref="DateTime"/> value is formatted as a string using the format specified by the
    /// <c>Format</c> property.</remarks>
    /// <param name="writer">The <see cref="Utf8JsonWriter"/> to write the JSON string to.</param>
    /// <param name="value">The <see cref="DateTime"/> value to be written.</param>
    /// <param name="options">The <see cref="JsonSerializerOptions"/> to use during serialization.</param>
    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(Format));
    }
}

public class JsonNullableDateConverter : JsonConverter<DateTime?>
{
    /// <summary>
    /// Represents the date and time format string used for formatting and parsing operations.
    /// </summary>
    /// <remarks>The format string follows the standard .NET date and time format conventions.  It specifies a
    /// pattern of "yyyy-MM-dd HH:mm:ss tt", which includes a four-digit year,  two-digit month, two-digit day, 24-hour
    /// clock hour, minutes, seconds, and an AM/PM designator.</remarks>

    private const string Format = "yyyy-MM-dd HH:mm:ss tt";

    /// <summary>
    /// Reads and converts a JSON string to a nullable <see cref="DateTime"/> value.
    /// </summary>
    /// <param name="reader">The <see cref="Utf8JsonReader"/> to read the JSON data from.</param>
    /// <param name="typeToConvert">The type of the object to convert. This parameter is not used in this implementation.</param>
    /// <param name="options">The <see cref="JsonSerializerOptions"/> to use during deserialization. This parameter is not used in this
    /// implementation.</param>
    /// <returns>A <see cref="DateTime"/> value if the JSON string represents a valid date and time; otherwise, <see
    /// langword="null"/> if the string is null, empty, or consists only of whitespace.</returns>
    public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var stringValue = reader.GetString();
        return string.IsNullOrWhiteSpace(stringValue) ? null : DateTime.Parse(stringValue);
    }

    /// <summary>
    /// Writes a nullable <see cref="DateTime"/> value to the specified <see cref="Utf8JsonWriter"/>  using the
    /// configured format.
    /// </summary>
    /// <param name="writer">The <see cref="Utf8JsonWriter"/> to which the value will be written. Cannot be <c>null</c>.</param>
    /// <param name="value">The nullable <see cref="DateTime"/> value to write. If <c>null</c>, no value is written.</param>
    /// <param name="options">The <see cref="JsonSerializerOptions"/> to use during serialization. This parameter is not used in this
    /// implementation.</param>
    public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value?.ToString(Format));
    }
}
