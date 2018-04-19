using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BAMCIS.GeoJSON
{
    /// <summary>
    /// The seven case-sensitive geometry types defined in RFC7946
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum GeometryType
    {
        Point,

        MultiPoint,

        LineString,

        MultiLineString,

        Polygon,

        MultiPolygon,

        GeometryCollection
    }
}
