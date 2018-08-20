using BAMCIS.GeoJSON.Serde;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BAMCIS.GeoJSON
{
    /// <summary>
    /// A polygon is formed with 1 or more linear rings, which are an enclosed LineString
    /// </summary>
    [JsonConverter(typeof(PolygonConverter))]
    public class Polygon : Geometry
    {
        #region Public Properties

        /// <summary>
        /// The coordinates are an array of linear ring coordinate arrays.
        ///
        /// For Polygons with more than one of these rings, the first MUST be
        /// the exterior ring, and any others MUST be interior rings.The
        /// exterior ring bounds the surface, and the interior rings(if
        /// present) bound holes within the surface.
        /// </summary>
        public IEnumerable<LinearRing> Coordinates { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new Polygon
        /// </summary>
        [JsonConstructor]
        public Polygon(IEnumerable<LinearRing> coordinates) : base(GeoJsonType.Polygon)
        {
            this.Coordinates = coordinates ?? throw new ArgumentNullException("coordinates");

            if (!this.Coordinates.Any())
            {
                throw new ArgumentOutOfRangeException("coordinates", "A polygon must have at least 1 linear ring.");
            }
        }

        #endregion

        #region Public Methods

        public static new Polygon FromJson(string json)
        {
            return JsonConvert.DeserializeObject<Polygon>(json);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj == null || this.GetType() != obj.GetType())
            {
                return false;
            }

            Polygon Other = (Polygon)obj;

            return this.Type == Other.Type &&
                this.Coordinates.SequenceEqual(Other.Coordinates) &&
                this.BoundingBox == Other.BoundingBox;
        }

        public override int GetHashCode()
        {
            return Hashing.Hash(this.Type, this.Coordinates, this.BoundingBox);
        }

        public static bool operator ==(Polygon left, Polygon right)
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

        public static bool operator !=(Polygon left, Polygon right)
        {
            return !(left == right);
        }

        #endregion
    }
}
