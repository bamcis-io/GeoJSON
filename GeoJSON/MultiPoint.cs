using BAMCIS.GeoJSON.Serde;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BAMCIS.GeoJSON
{
    /// <summary>
    /// For type "MultiPoint", the "coordinates" member is an array of positions.
    /// </summary>
    [JsonConverter(typeof(InheritanceBlockerConverter))]
    public class MultiPoint : Geometry
    {
        #region Public Properties

        /// <summary>
        /// The coordinates of a multipoint are an array of positions
        /// </summary>
        [JsonProperty(PropertyName = "coordinates")]
        public IEnumerable<Position> Coordinates { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new multipoint object
        /// </summary>
        /// <param name="coordinates"></param>
        public MultiPoint(IEnumerable<Position> coordinates, IEnumerable<double> boundingBox = null) : base(GeoJsonType.MultiPoint, coordinates.Any(x => x.HasElevation()), boundingBox)
        {
            this.Coordinates = coordinates ?? throw new ArgumentNullException("coordinates");
        }

        #endregion

        #region Public Methods

        public static new MultiPoint FromJson(string json)
        {
            return JsonConvert.DeserializeObject<MultiPoint>(json);
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

            MultiPoint other = (MultiPoint)obj;

            bool bBoxEqual = true;

            if (this.BoundingBox != null && other.BoundingBox != null)
            {
                bBoxEqual = this.BoundingBox.SequenceEqual(other.BoundingBox);
            }
            else
            {
                bBoxEqual = (this.BoundingBox == null && other.BoundingBox == null);
            }

            bool coordinatesEqual = true;

            if (this.Coordinates != null && other.Coordinates != null)
            {
                coordinatesEqual = this.Coordinates.SequenceEqual(other.Coordinates);
            }
            else
            {
                coordinatesEqual = (this.Coordinates == null && other.Coordinates == null);
            }

            return this.Type == other.Type &&
                coordinatesEqual &&
                bBoxEqual;
        }

        public override int GetHashCode()
        {
            return Hashing.Hash(this.Type, this.Coordinates, this.BoundingBox);
        }

        public static bool operator ==(MultiPoint left, MultiPoint right)
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

        public static bool operator !=(MultiPoint left, MultiPoint right)
        {
            return !(left == right);
        }

        #endregion
    }
}
