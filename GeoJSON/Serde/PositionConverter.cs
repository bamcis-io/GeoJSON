using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace BAMCIS.GeoJSON.Serde
{
    /// <summary>
    /// Converts a position to an array of coordinates and back
    /// </summary>
    public class PositionConverter : JsonConverter
    {
        #region Public Properties

        public override bool CanRead => true;

        public override bool CanWrite => true;

        #endregion

        #region Public Methods

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Position);
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
            // This is an array of doubles
            JArray Token = JArray.Load(reader);

            double Longitude = Token.ElementAt(0).ToObject<double>(serializer);
            double Latitude = Token.ElementAt(1).ToObject<double>(serializer);
            double Elevation = double.NaN;

            if (Token.Count == 3)
            {
                Elevation = Token.ElementAt(2).ToObject<double>(serializer);
            }

            return new Position(Longitude, Latitude, Elevation);
        }

        /// <summary>
        /// Takes the position object and converts it to an array of doubles
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="serializer"></param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Position Pos = (Position)value;

            if (Pos.HasElevation())
            {
                JToken.FromObject(new double[3] { Pos.Longitude, Pos.Latitude, Pos.Elevation }).WriteTo(writer);
            }
            else
            {
                JToken.FromObject(new double[2] { Pos.Longitude, Pos.Latitude }).WriteTo(writer);
            }
        }

        #endregion
    }
}
