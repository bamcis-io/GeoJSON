using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace BAMCIS.GeoJSON.Serde
{
    /// <summary>
    /// Converts a position to an array of coordinates and back
    /// </summary>
    public class PointConverter : JsonConverter
    {
        #region Public Properties

        public override bool CanRead => true;

        public override bool CanWrite => true;

        #endregion

        #region Public Methods

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(PointConverter);
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
            JObject token = JObject.Load(reader);

            Coordinate coordinate = token.GetValue("coordinates", StringComparison.OrdinalIgnoreCase)
                                    .ToObject<Coordinate>(serializer);

            var point = new Point(coordinate);

            return point;

        }

        /// <summary>
        /// Takes the position object and converts it to an array of doubles
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="serializer"></param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Point point = (Point) value;

            JToken.FromObject(new
            {
                type = point.Type,
                coordinates = point.Coordinates
            }).WriteTo(writer);
        }

        #endregion
    }
}
