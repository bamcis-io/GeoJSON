using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BAMCIS.GeoJSON.Serde
{
    class MultiPointConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            // this converter can be applied to any type
            return true;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var token = JObject.Load(reader);
            

            var points = token.GetValue("points",
                                             StringComparison.OrdinalIgnoreCase)
                                   .ToObject<IEnumerable<Coordinate>>(serializer)
                                   .ToList();

            // Take this array of arrays of arrays and create linear rings
            // and use those to create create polygons
            return new MultiPoint(points.Select(c => c.ToPoint()).ToList());
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var multiPoint = (MultiPoint) value;

            var coordinates = multiPoint.Points.Select(p => p.Coordinates).ToList();

            JToken.FromObject(new
            {
                type = multiPoint.Type,
                points = coordinates
            }).WriteTo(writer);
        }
    }
}
