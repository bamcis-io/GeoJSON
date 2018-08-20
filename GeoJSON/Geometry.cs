﻿using BAMCIS.GeoJSON.Serde;
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

        private static readonly Dictionary<Type, GeoJsonType> TypeToDerivedType;
        private static readonly Dictionary<GeoJsonType, Type> DerivedTypeToType;

        #endregion

        #region Constructors

        /// <summary>
        /// Builds the dictionary that maintains the type mapping
        /// </summary>
        static Geometry()
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
            };

            DerivedTypeToType = TypeToDerivedType.ToDictionary(pair => pair.Value, pair => pair.Key);
        }

        /// <summary>
        /// Each inherited class must implement this constructor
        /// </summary>
        [JsonConstructor]
        protected Geometry(GeoJsonType type) : base(type)
        {
            if (!DerivedTypeToType.ContainsKey(type))
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
        public new static Type GetType(GeoJsonType type)
        {
            if (DerivedTypeToType.ContainsKey(type))
            {
                return DerivedTypeToType[type];
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
