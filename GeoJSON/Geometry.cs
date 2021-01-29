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

        private static readonly Dictionary<Type, GeoJsonType> typeToDerivedType;
        private static readonly Dictionary<GeoJsonType, Type> derivedTypeToType;

        #endregion

        #region Constructors

        /// <summary>
        /// Builds the dictionary that maintains the type mapping
        /// </summary>
        static Geometry()
        {
            typeToDerivedType = new Dictionary<Type, GeoJsonType>()
            {
                { typeof(LineString), GeoJsonType.LineString },
                { typeof(MultiLineString), GeoJsonType.MultiLineString },
                { typeof(MultiPoint), GeoJsonType.MultiPoint },
                { typeof(MultiPolygon), GeoJsonType.MultiPolygon },
                { typeof(Point), GeoJsonType.Point },
                { typeof(Polygon), GeoJsonType.Polygon },
                { typeof(GeometryCollection), GeoJsonType.GeometryCollection },
            };

            derivedTypeToType = typeToDerivedType.ToDictionary(pair => pair.Value, pair => pair.Key);
        }

        /// <summary>
        /// Each inherited class must implement this constructor
        /// </summary>
        /// <param name="type">The GeoJson type</param>
        [JsonConstructor]
        protected Geometry(GeoJsonType type, bool is3D, IEnumerable<double> boundingBox = null) : base(type, is3D, boundingBox)
        {
            if (!derivedTypeToType.ContainsKey(type))
            {
                throw new ArgumentException($"The type {type} is not a valid geometry type.");
            }
        }

        #endregion

        #region Public Methods

        public static new Geometry FromJson(string json)
        {
            return JsonConvert.DeserializeObject<Geometry>(json);
        }

        /// <summary>
        /// Gets the appropriate class type corresponding to the enum
        /// representing the type
        /// </summary>
        /// <param name="type">The GeoJson type</param>
        /// <returns>The .NET type that corresponds to the GeoJson type</returns>
        public new static Type GetType(GeoJsonType type)
        {
            if (derivedTypeToType.ContainsKey(type))
            {
                return derivedTypeToType[type];
            }
            else
            {
                throw new ArgumentException($"The type {type} is not a valid geometry type.");
            }
        }

        public static bool operator ==(Geometry left, Geometry right)
        {
            if (ReferenceEquals(left, right))
            {
                return true;
            }

            if (right is null || left is null)
            {
                return false;
            }

            return left.Equals(right);
        }

        public static bool operator !=(Geometry left, Geometry right)
        {
            return !(left == right);
        }

        public abstract override bool Equals(object obj);

        public abstract override int GetHashCode();

        #endregion
    }
}
