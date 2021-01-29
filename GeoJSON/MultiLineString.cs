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

            MultiLineString other = (MultiLineString)obj;

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
