using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BAMCIS.GeoJSON.Serde
{
    /// <summary>
    /// Converts Polygon geometry objects
    /// </summary>
    public class PolygonConverter : JsonConverter
    {
        #region Public Properties

        public override bool CanRead => true;

        public override bool CanWrite => true;

        #endregion

        #region Public Methods

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Polygon);
        }

        /// <summary>
        /// This takes the array of arrays and recasts them back to linear ring objects
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="objectType"></param>
        /// <param name="existingValue"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject token = JObject.Load(reader);

            IEnumerable<IEnumerable<Coordinate>> coordinates = token.GetValue("Coordinates", StringComparison.OrdinalIgnoreCase).ToObject<IEnumerable<IEnumerable<Coordinate>>>(serializer);

            // Take this array of arrays of arrays and create linear rings
            // and use those to create create polygons
            return new Polygon(new LinearRing(coordinates));
        }

        /// <summary>
        /// This flattens the coordinates property into an array of arrays
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="serializer"></param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Polygon poly = (Polygon)value;

            var coordinates = poly.LinearRings.Select(lineRing => lineRing.Points.Select(p => p.Coordinates));

            JToken.FromObject(new
            {
                type = poly.Type,
                coordinates = coordinates
            }).WriteTo(writer);
        }

        #endregion
    }
}
