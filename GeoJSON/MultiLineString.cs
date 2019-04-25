using BAMCIS.GeoJSON.Serde;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BAMCIS.GeoJSON
{
    /// <summary>
    /// For type "MultiLineString", the "coordinates" member is an array of
    /// LineString coordinate arrays.
    /// </summary>
    [JsonConverter(typeof(MultiLineStringConverter))]
    public class MultiLineString : Geometry
    {
        #region Public Properties

        /// <summary>
        /// For type "MultiLineString", the "coordinates" member is an array of
        /// LineString coordinate arrays.
        /// </summary>
        [JsonProperty(PropertyName = "coordinates")]
        public IEnumerable<LineString> Coordinates { get; }

        #endregion

        #region Constructors 

        /// <summary>
        /// Creates a new MultiLineString
        /// </summary>
        /// <param name="coordinates">The line strings that make up the object</param>
        [JsonConstructor]
        public MultiLineString(IEnumerable<LineString> coordinates, IEnumerable<double> boundingBox = null) : base(GeoJsonType.MultiLineString, coordinates.Any(x => x.IsThreeDimensional()), boundingBox)
        {
            this.Coordinates = coordinates ?? throw new ArgumentNullException("coordinates");

            if (this.Coordinates.Count() < 2)
            {
                throw new ArgumentOutOfRangeException("coordinates", "A LineString must have at least two positions.");
            }
        }

        #endregion

        #region Public Methods

        public static new MultiLineString FromJson(string json)
        {
            return JsonConvert.DeserializeObject<MultiLineString>(json);
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

            MultiLineString Other = (MultiLineString)obj;

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

        public static bool operator ==(MultiLineString left, MultiLineString right)
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

        public static bool operator !=(MultiLineString left, MultiLineString right)
        {
            return !(left == right);
        }

        #endregion
    }
}
