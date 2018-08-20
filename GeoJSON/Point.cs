using BAMCIS.GeoJSON.Serde;
using Newtonsoft.Json;
using System;

namespace BAMCIS.GeoJSON
{
    /// <summary>
    /// For type "Point", the "coordinates" member is a single position.
    /// </summary>
    [JsonConverter(typeof(InheritanceBlockerConverter))]
    public class Point : Geometry
    {
        #region Public Properties

        /// <summary>
        /// For type "Point", the "coordinates" member is a single position.
        /// </summary>
        public Position Coordinates { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new point with the provided coordinates
        /// </summary>
        [JsonConstructor]
        public Point(Position coordinates) : base(GeoJsonType.Point)
        {
            this.Coordinates = coordinates ?? throw new ArgumentNullException("coordinates");
        }

        #endregion

        #region Public Methods

        public static new Point FromJson(string json)
        {
            return JsonConvert.DeserializeObject<Point>(json);
        }

        /// <summary>
        /// Gets the longitude or easting of the point
        /// </summary>
        public double GetLongitude()
        {
            return this.Coordinates.Longitude;
        }

        /// <summary>
        /// Gets the latitude or northing of the point
        /// </summary>
        public double GetLatitude()
        {
            return this.Coordinates.Latitude;
        }

        /// <summary>
        /// Gets the elevation of the point if it exists
        /// in the coordinates.
        /// </summary>
        public bool TryGetElevation(out double elevation)
        {
            if (this.Coordinates.HasElevation())
            {
                elevation = this.Coordinates.Elevation;
                return true;
            }

            elevation = double.NaN;
            return false;
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

            Point Other = (Point)obj;

            return this.Type == Other.Type &&
                this.Coordinates == Other.Coordinates &&
                this.BoundingBox == Other.BoundingBox;
        }

        public override int GetHashCode()
        {
            return Hashing.Hash(this.Type, this.Coordinates, this.BoundingBox);
        }

        public static bool operator ==(Point left, Point right)
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

        public static bool operator !=(Point left, Point right)
        {
            return !(left == right);
        }

        #endregion
    }
}
