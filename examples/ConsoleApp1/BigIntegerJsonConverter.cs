using System;
using System.Globalization;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ConsoleApp1
{
    /// <summary>
    /// See https://stackoverflow.com/questions/64788895/serialising-biginteger-using-system-text-json
    /// </summary>
    public class BigIntegerJsonConverter : JsonConverter<BigInteger>
    {
        public override BigInteger Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.Number)
            {
                throw new JsonException(string.Format("Found token {0} but expected token {1}", reader.TokenType, JsonTokenType.Number));
            }

            using var doc = JsonDocument.ParseValue(ref reader);
            return BigInteger.Parse(doc.RootElement.GetRawText(), NumberFormatInfo.InvariantInfo);
        }

        public override void Write(Utf8JsonWriter writer, BigInteger value, JsonSerializerOptions options)
        {
            var s = value.ToString(NumberFormatInfo.InvariantInfo);
            using var doc = JsonDocument.Parse(s);
            doc.WriteTo(writer);
        }
    }
}