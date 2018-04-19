using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BAMCIS.GeoJSON
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum GeoJsonType
    {
        /// <summary>
        /// GeoJSON supports the following geometry types:
        /// Point, LineString, Polygon, MultiPoint, MultiLineString,
        /// MultiPolygon, and GeometryCollection
        /// </summary>
        GEOMETRY,

        /// <summary>
        /// Feature objects in GeoJSON contain a Geometry object with one of the
        /// above geometry types and additional members.
        /// </summary>
        FEATURE,

        /// <summary>
        /// A FeatureCollection object contains an array of Feature objects.
        /// </summary>
        FEATURECOLLECTION
    }
}
