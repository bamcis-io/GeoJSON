using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAMCIS.GeoJSON.Serde
{
    public class LinearRingConverter : JsonConverter
    {
        #region Public Properties

        public override bool CanRead => true;

        public override bool CanWrite => true;

        #endregion

        #region Public Methods

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(MultiLineString);
        }

        /// <summary>
        /// This takes the array of arrays and recasts them back to line string objects
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="objectType"></param>
        /// <param name="existingValue"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject token = JObject.Load(reader);

            var coordinates = token.GetValue("coordinates", StringComparison.OrdinalIgnoreCase).ToObject<IEnumerable<Coordinate>>(serializer);

            var lineString = new LinearRing(coordinates);

            return lineString;
        }

        /// <summary>
        /// This flattens the coordinates property into an array of arrays
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="serializer"></param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var lineRing = (LinearRing) value;

            JToken.FromObject(new
            {
                type = lineRing.Type,
                coordinates = lineRing.Points.Select(p => p.Coordinates).ToList()
            }).WriteTo(writer);
        }

        #endregion Public Methods
    }
}
