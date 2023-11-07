using BAMCIS.GeoJSON.Serde;
using BAMCIS.GeoJSON.Wkb;
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

        private static readonly Dictionary<Type, GeoJsonType> typeToDerivedType;
        private static readonly Dictionary<GeoJsonType, Type> derivedTypeToType;
        private readonly bool is3D;

        #endregion

        #region Public Properties

        /// <summary>
        /// The type of the geojson object
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        public GeoJsonType Type { get; }


        #endregion

        #region Constructors

        /// <summary>
        /// Builds the dictionary that maintains the type mapping
        /// </summary>
        static GeoJson()
        {
            typeToDerivedType = new Dictionary<Type, GeoJsonType>()
            {
                { typeof(LineSegment), GeoJsonType.LineSegment },
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

            derivedTypeToType = typeToDerivedType.ToDictionary(pair => pair.Value, pair => pair.Key);
        }

        /// <summary>
        /// Base constructor that all derived classes must implement
        /// </summary>
        /// <param name="type">The type of the GeoJson object</param>
        protected GeoJson(GeoJsonType type, bool is3D)
        {
            this.Type = type;
            this.is3D = is3D;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Indicates that at least one of the coordinates in the GeoJson has an elevation
        /// </summary>
        /// <returns></returns>
        public bool IsThreeDimensional()
        {
            return this.is3D;
        }

        /// <summary>
        /// Gets the appropriate class type corresponding to the enum
        /// representing the type
        /// </summary>
        /// <param name="type">The GeoJson type of the </param>
        /// <returns>The .NET type that corresponds to the GeoJson type</returns>
        public static Type GetType(GeoJsonType type)
        {
            return derivedTypeToType[type];
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
