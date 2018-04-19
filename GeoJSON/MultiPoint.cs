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
        public IEnumerable<Position> Coordinates { get; }

        #endregion

        #region Constructors 

        /// <summary>
        /// Creates a new multipoint object
        /// </summary>
        /// <param name="coordinates"></param>
        public MultiPoint(IEnumerable<Position> coordinates) : base(GeoJsonType.MultiPoint)
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

            MultiPoint Other = (MultiPoint)obj;

            return this.Type == Other.Type &&
                this.Coordinates.SequenceEqual(Other.Coordinates) &&
                this.BoundingBox == Other.BoundingBox;
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
