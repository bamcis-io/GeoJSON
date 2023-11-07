using Newtonsoft.Json;
using System;

namespace BAMCIS.GeoJSON.Serde
{
    /// <summary>
    /// Provides conversion of a FeatureId object to either a string or integer as the value
    /// for a feature's "id" property
    /// </summary>
    public class FeatureIdConverter : JsonConverter
    {
        #region Public Properties

        public override bool CanRead => true;

        public override bool CanWrite => true;

        #endregion

        #region Public Methods

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Coordinate);
        }

        /// <summary>
        /// Reads an array of doubles and creates a Position object
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="objectType"></param>
        /// <param name="existingValue"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            switch (reader.TokenType)
            {
                case JsonToken.String:
                    {
                        return new FeatureId(reader.Value as string);
                    }
                case JsonToken.Integer:
                    {
                        return new FeatureId((Int64)reader.Value);
                    }
                case JsonToken.Null:
                    {
                        return null;
                    }
                default:
                    {
                        throw new FormatException("The feature id was provided in an unexpected format.");
                    }
            }
        }

        /// <summary>
        /// Takes the position object and converts it to an array of doubles
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="serializer"></param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            FeatureId id = (FeatureId)value;

            if (id.GetOriginalType() == typeof(string))
            {
                writer.WriteValue(id.Value);
            }
            else
            {
                writer.WriteValue(Int64.Parse(id.Value));
            }
        }

        #endregion
    }
}
