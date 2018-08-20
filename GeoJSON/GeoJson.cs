using BAMCIS.GeoJSON.Serde;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BAMCIS.GeoJSON
{
    /// <summary>
    /// A base abstract class for the implementation of GeoJson
    /// </summary>
    [JsonConverter(typeof(GeoJsonConverter))]
    public abstract class GeoJson
    {
        #region Private Fields

        private static readonly Dictionary<Type, GeoJsonType> TypeToDerivedType;
        private static readonly Dictionary<GeoJsonType, Type> DerivedTypeToType;

        #endregion

        #region Public Properties

        /// <summary>
        /// The type of the geojson object
        /// </summary>
        public GeoJsonType Type { get; }

        /// <summary>
        ///  A GeoJSON object MAY have a member named "bbox" to include
        /// information on the coordinate range for its Geometries, Features, or
        /// FeatureCollections.The value of the bbox member MUST be an array of
        /// length 2*n where n is the number of dimensions represented in the
        /// contained geometries, with all axes of the most southwesterly point
        /// followed by all axes of the more northeasterly point.The axes order
        /// of a bbox follows the axes order of geometries.
        /// </summary>
        [JsonProperty(PropertyName = "bbox", NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<double> BoundingBox { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Builds the dictionary that maintains the type mapping
        /// </summary>
        static GeoJson()
        {
            TypeToDerivedType = new Dictionary<Type, GeoJsonType>()
            {
                { typeof(LineString), GeoJsonType.LineString },
                { typeof(MultiLineString), GeoJsonType.MultiLineString },
                { typeof(MultiPoint), GeoJsonType.MultiPoint },
                { typeof(MultiPolygon), GeoJsonType.MultiPolygon },
                { typeof(Point), GeoJsonType.Point },
                { typeof(Polygon), GeoJsonType.Polygon },
                { typeof(GeometryCollection), GeoJsonType.GeometryCollection },
                { typeof(Feature), GeoJsonType.Feature },
                { typeof(FeatureCollection), GeoJsonType.FeatureCollection }
            };

            DerivedTypeToType = TypeToDerivedType.ToDictionary(pair => pair.Value, pair => pair.Key);
        }

        /// <summary>
        /// Base constructor that all derived classes must implement
        /// </summary>
        /// <param name="type">The type of the GeoJson object</param>
        protected GeoJson(GeoJsonType type)
        {
            this.Type = type;
            this.BoundingBox = null;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the appropriate class type corresponding to the enum
        /// representing the type
        /// </summary>
        /// <param name="type">The GeoJson type of the </param>
        /// <returns>The .NET type that corresponds to the GeoJson type</returns>
        public static Type GetType(GeoJsonType type)
        {
            return DerivedTypeToType[type];
        }

        public abstract override bool Equals(object obj);

        public abstract override int GetHashCode();

        public virtual string ToJson()
        {
            return this.ToJson(Formatting.None);
        }

        public virtual string ToJson(Formatting formatting)
        {
            return JsonConvert.SerializeObject(this, formatting);
        }

        /// <summary>
        /// Deserializes the json to a GeoJson object
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static GeoJson FromJson(string json) 
        {
            return JsonConvert.DeserializeObject<GeoJson>(json);
        }

        #endregion
    }
}
