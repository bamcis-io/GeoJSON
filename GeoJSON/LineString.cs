using BAMCIS.GeoJSON.Serde;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BAMCIS.GeoJSON
{
    /// <summary>
    /// For type "LineString", the "coordinates" member is an array of two or
    /// more positions.
    /// </summary>
    [JsonConverter(typeof(InheritanceBlockerConverter))]
    public class LineString : Geometry
    {
        #region Public Properties

        /// <summary>
        /// The coordinates of a linestring are an array of positions
        /// </summary>
        public IEnumerable<Position> Coordinates { get; }

        #endregion

        #region Constructors 

        /// <summary>
        /// Creates a new LineString
        /// </summary>
        /// <param name="coordinates"></param>
        public LineString(IEnumerable<Position> coordinates) : base(GeoJsonType.LineString)
        {
            this.Coordinates = coordinates ?? throw new ArgumentNullException("coordinates");

            if (this.Coordinates.Count() < 2)
            {
                throw new ArgumentOutOfRangeException("coordinates", "A LineString must have at least two positions.");
            }
        }

        #endregion

        #region Public Methods

        public static new LineString FromJson(string json)
        {
            return JsonConvert.DeserializeObject<LineString>(json);
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

            LineString Other = (LineString)obj;

            return this.Type == Other.Type &&
                this.Coordinates.SequenceEqual(Other.Coordinates) &&
                this.BoundingBox == Other.BoundingBox;
        }

        public override int GetHashCode()
        {
            return Hashing.Hash(this.Type, this.Coordinates, this.BoundingBox);
        }

        public static bool operator ==(LineString left, LineString right)
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

        public static bool operator !=(LineString left, LineString right)
        {
            return !(left == right);
        }

        #endregion
    }
}
