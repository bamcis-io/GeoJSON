using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BAMCIS.GeoJSON.Serde
{
    /// <summary>
    /// Converts a MultiLineString GeoJSON object to and from JSON
    /// </summary>
    public class MultiLineStringConverter : JsonConverter
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
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject Token = JObject.Load(reader);

            IEnumerable<IEnumerable<Position>> Coordinates = Token.GetValue("coordinates", StringComparison.OrdinalIgnoreCase).ToObject<IEnumerable<IEnumerable<Position>>>(serializer);

            return new MultiLineString(Coordinates.Select(x => new LineString(x)));
        }

        /// <summary>
        /// This flattens the coordinates property into an array of arrays
        /// </summary>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            MultiLineString Mls = (MultiLineString)value;

            JToken.FromObject(new
            {
                type = Mls.Type,
                coordinates = Mls.Coordinates.Select(x => x.Coordinates)
            }).WriteTo(writer);
        }

        #endregion
    }
}
