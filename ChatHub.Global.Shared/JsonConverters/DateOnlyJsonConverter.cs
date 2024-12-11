using ChatHub.Global.Shared.Commons;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ChatHub.Global.Shared.JsonConverters
{
    public class DateOnlyJsonConverter : JsonConverter<DateOnly>
    {
        public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString()!;
            return DateOnly.ParseExact(value, Constant.DATE_TIME_FORMAT_MMddyyyy);
        }

        public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(Constant.DATE_TIME_FORMAT_MMddyyyy));
        }
    }
}
