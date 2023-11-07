using BAMCIS.GeoJSON.Serde;
using BAMCIS.GeoJSON.Wkb;
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
    public abstract class Geometry: GeoJson
    {
        #region Private Fields

        private static readonly Dictionary<Type, GeoJsonType> typeToDerivedType;
        private static readonly Dictionary<GeoJsonType, Type> derivedTypeToType;

        [JsonProperty(PropertyName = "BoundingBox")]
        [JsonIgnore]
        public abstract Rectangle BoundingBox { get; }


        #endregion

        #region Constructors

        /// <summary>
        /// Builds the dictionary that maintains the type mapping
        /// </summary>
        static Geometry()
        {
            typeToDerivedType = new Dictionary<Type, GeoJsonType>()
            {
                { typeof(LineSegment), GeoJsonType.LineSegment },
                { typeof(LineString), GeoJsonType.LineString },
                { typeof(LinearRing), GeoJsonType.LinearRing },
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
        protected Geometry(GeoJsonType type, bool is3D) : base(type, is3D)
        {
            ConstructorEvaluator(type);
        }


        /// <summary>
        /// Converts angles from Radians to Degrees
        /// </summary>
        /// <param name="radians"></param>
        /// <returns></returns>
        public static double RadiansToDegrees(double radians)
        {
            return ( radians * 180 / Math.PI );
        }

        /// <summary>
        /// Converts angles from Degrees to Radians
        /// </summary>
        /// <param name="radians"></param>
        /// <returns></returns>
        public static double DegreesToRadians(double radians)
        {
            return ( radians * 180 / Math.PI );
        }

        /// <summary>
        /// Each inherited class must implement this constructor
        /// </summary>
        /// <param name="type">The GeoJson type</param>
        protected Geometry(IEnumerable<GeoJson> geometries = null) : base(GeoJsonType.Feature, geometries.FirstOrDefault()?.IsThreeDimensional() ?? false)
        {
            
        }

        protected static void ConstructorEvaluator(GeoJsonType type)
        {
            if (!derivedTypeToType.ContainsKey(type))
            {
                throw new ArgumentException($"The type {type} is not a valid geometry type.");
            }
        }

        #endregion

        #region Public Methods

        #region Converters

        public static new Geometry FromJson(string json)
        {
            var geometry = JsonConvert.DeserializeObject<Geometry>(json);

            return geometry;
        }

        /// <summary>
        /// Deserialize WKB byte array to Geometry.
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static Geometry FromWkb(byte[] bytes)
        {
            return WkbConverter.FromBinary(bytes);
        }

        /// <summary>
        /// Deserialize WKB byte array to Geometry.
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static T FromWkb<T>(byte[] bytes) where T : Geometry
        {
            return WkbConverter.FromBinary<T>(bytes);
        }

        /// <summary>
        /// Serialize this geometry object to WKB
        /// </summary>
        /// <param name="endianness">The default is LITTLE</param>
        /// <returns></returns>
        public byte[] ToWkb(Endianness endianness = Endianness.LITTLE)
        {
            return WkbConverter.ToBinary(this, endianness);
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

        #endregion Converters

        #region Equality Evaluators

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


        #endregion Equality Evaluators

        #endregion
    }

}
