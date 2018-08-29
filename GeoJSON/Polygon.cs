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
        /// <param name="coordinates">The linear rings that make up the polygon</param>
        [JsonConstructor]
        public Polygon(IEnumerable<LinearRing> coordinates, IEnumerable<double> boundingBox = null) : base(GeoJsonType.Polygon, coordinates.Any(x => x.IsThreeDimensional()), boundingBox)
        {
            this.Coordinates = coordinates ?? throw new ArgumentNullException("coordinates");

            if (!this.Coordinates.Any())
            {
                throw new ArgumentOutOfRangeException("coordinates", "A polygon must have at least 1 linear ring.");
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Deserializes the json into a Polygon
        /// </summary>
        /// <param name="json">The json to deserialize</param>
        /// <returns>A Polygon object</returns>
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

            bool BBoxEqual = true;

            if (this.BoundingBox != null && Other.BoundingBox != null)
            {
                BBoxEqual = this.BoundingBox.SequenceEqual(Other.BoundingBox);
            }
            else
            {
                BBoxEqual = (this.BoundingBox == null && Other.BoundingBox == null);
            }

            bool CoordinatesEqual = true;

            if (this.Coordinates != null && Other.Coordinates != null)
            {
                CoordinatesEqual = this.Coordinates.SequenceEqual(Other.Coordinates);
            }
            else
            {
                CoordinatesEqual = (this.Coordinates == null && Other.Coordinates == null);
            }

            return this.Type == Other.Type &&
                CoordinatesEqual &&
                BBoxEqual;
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
