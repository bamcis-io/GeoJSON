using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BAMCIS.GeoJSON
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum GeoJsonType
    {
        Point,

        MultiPoint,

        LineString,

        MultiLineString,

        Polygon,

        MultiPolygon,

        GeometryCollection,

        /// <summary>
        /// Feature objects in GeoJSON contain a Geometry object with one of the
        /// above geometry types and additional members.
        /// </summary>
        Feature,

        /// <summary>
        /// A FeatureCollection object contains an array of Feature objects.
        /// </summary>
        FeatureCollection
    }
}
