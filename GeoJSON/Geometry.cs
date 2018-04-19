using BAMCIS.GeoJSON.Serde;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BAMCIS.GeoJSON
{
    /// <summary>
    /// A base abstract class for geometry types
    /// </summary>
    [JsonConverter(typeof(GeometryConverter))]
    public abstract class Geometry : GeoJson
    {
        #region Private Fields

        private static readonly Dictionary<Type, GeometryType> TypeToDerivedType;
        private static readonly Dictionary<GeometryType, Type> DerivedTypeToType;

        #endregion

        #region Public Properties

        /// <summary>
        /// Overrides the base GeoJson and defines its own type
        /// for the specific geometry
        /// </summary>
        public new GeometryType Type { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Builds the dictionary that maintains the type mapping
        /// </summary>
        static Geometry()
        {
            TypeToDerivedType = new Dictionary<Type, GeometryType>()
            {
                { typeof(LineString), GeometryType.LineString },
                { typeof(MultiLineString), GeometryType.MultiLineString },
                { typeof(MultiPoint), GeometryType.MultiPoint },
                { typeof(MultiPolygon), GeometryType.MultiPolygon },
                { typeof(Point), GeometryType.Point },
                { typeof(Polygon), GeometryType.Polygon },
                { typeof(GeometryCollection), GeometryType.GeometryCollection },
            };

            DerivedTypeToType = TypeToDerivedType.ToDictionary(pair => pair.Value, pair => pair.Key);
        }

        /// <summary>
        /// Each inherited class must implement this constructor
        /// </summary>
        /// <param name="type"></param>
        [JsonConstructor]
        protected Geometry(GeometryType type) : base(GeoJsonType.GEOMETRY)
        {
            this.Type = type;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the appropriate class type corresponding to the enum
        /// representing the type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Type GetType(GeometryType type)
        {
            return DerivedTypeToType[type];
        }

        #endregion
    }
}
