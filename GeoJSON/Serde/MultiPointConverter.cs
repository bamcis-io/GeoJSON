using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            // we currently support only writing of JSON
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var obj = value as MultiPoint;

            if (obj == null)
            {
                serializer.Serialize(writer, null);
                return;
            }

            // find all properties that are points.
            var points = obj.GetType().GetProperties().Where(p => p.Name == "Points");

            writer.WriteStartObject();

            foreach (var property in points)
            {
                // write property name
                writer.WritePropertyName(property.Name);
                // let the serializer serialize the obj itself
                // (so this converter will work with any other type, not just int)
                var propertyValue = property.GetValue(obj, null);
                serializer.Serialize(writer, propertyValue);
            }

            writer.WriteEndObject();
        }
    }
}
