using BAMCIS.GeoJSON.Serde;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BAMCIS.GeoJSON
{
    /// <summary>
    /// For type "MultiPolygon", the "coordinates" member is an array of
    /// Polygon coordinate arrays.
    /// </summary>
    [JsonConverter(typeof(MultiPolygonConverter))]
    public class MultiPolygon : Geometry
    {
        #region Public Properties

        /// <summary>
        /// The coordinates are an array of polygons.
        /// </summary>
        [JsonProperty(PropertyName = "coordinates")]
        public IEnumerable<Polygon> Coordinates { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new MultiPolygon
        /// </summary>
        /// <param name="coordinates">The coordinates that make up the multi polygon</param>
        [JsonConstructor]
        public MultiPolygon(IEnumerable<Polygon> coordinates, IEnumerable<double> boundingBox = null) : base(GeoJsonType.MultiPolygon, coordinates.Any(x => x.IsThreeDimensional()), boundingBox)
        {
            this.Coordinates = coordinates ?? throw new ArgumentNullException("coordinates");

            if (!this.Coordinates.Any())
            {
                throw new ArgumentOutOfRangeException("coordinates", "A MultiPolygon must have at least 1 polygon.");
            }
        }

        #endregion

        #region Public Methods
        public static new MultiPolygon FromJson(string json)
        {
            return JsonConvert.DeserializeObject<MultiPolygon>(json);
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

            MultiPolygon Other = (MultiPolygon)obj;

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

        public static bool operator ==(MultiPolygon left, MultiPolygon right)
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

        public static bool operator !=(MultiPolygon left, MultiPolygon right)
        {
            return !(left == right);
        }

        #endregion
    }
}
