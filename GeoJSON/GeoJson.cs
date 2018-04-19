using System;
using System.Collections.Generic;
using System.Linq;

namespace BAMCIS.GeoJSON
{
    /// <summary>
    /// A base abstract class for the implementation of GeoJson
    /// </summary>
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

        #endregion

        #region Constructors

        /// <summary>
        /// Builds the dictionary that maintains the type mapping
        /// </summary>
        static GeoJson()
        {
            TypeToDerivedType = new Dictionary<Type, GeoJsonType>()
            {
                { typeof(Geometry), GeoJsonType.GEOMETRY },
                { typeof(Feature), GeoJsonType.FEATURE },
                { typeof(FeatureCollection), GeoJsonType.FEATURECOLLECTION }
            };

            DerivedTypeToType = TypeToDerivedType.ToDictionary(pair => pair.Value, pair => pair.Key);
        }

        /// <summary>
        /// Base constructor that all derived classes must implement
        /// </summary>
        /// <param name="type"></param>
        protected GeoJson(GeoJsonType type)
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
        public static Type GetType(GeoJsonType type)
        {
            return DerivedTypeToType[type];
        }

        #endregion
    }
}
