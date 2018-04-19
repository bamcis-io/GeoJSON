using Newtonsoft.Json;
using System;

namespace BAMCIS.GeoJSON.Serde
{
    /// <summary>
    /// This converter is just used to block the base Geometry converter
    /// so that and inherited classes use the default serde when the 
    /// GeometryConverter calls ToObject() on them
    /// </summary>
    public class InheritanceBlockerConverter : JsonConverter
    {
        #region Public Properties

        public override bool CanRead => false;

        public override bool CanWrite => false;

        #endregion

        #region Public Methods

        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
