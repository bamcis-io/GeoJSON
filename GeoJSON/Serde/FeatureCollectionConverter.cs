using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAMCIS.GeoJSON.Serde
{
    
    class FeatureCollectionConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            // this converter can be applied to any type
            return true;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var token = JObject.Load(reader);


            var features = token.GetValue("features",
                                             StringComparison.OrdinalIgnoreCase)
                                   .ToObject<IEnumerable<Feature>>(serializer)
                                   .ToList();

            return new FeatureCollection(features);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var featCol = (FeatureCollection) value;
            
            // Aqui há um erro
            JToken.FromObject(new
            {
                type = featCol.Type,
                features = featCol.Features
            }).WriteTo(writer);
        }
    }


}
