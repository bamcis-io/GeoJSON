using BAMCIS.GeoJSON.Serde;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

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
        [JsonProperty(PropertyName = "coordinates")]
        public Position Coordinates { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new point with the provided coordinates
        /// </summary>
        /// <param name="coordinates">The position of this point</param>
        [JsonConstructor]
        public Point(Position coordinates, IEnumerable<double> boundingBox = null) : base(GeoJsonType.Point, coordinates.HasElevation(), boundingBox)
        {
            this.Coordinates = coordinates ?? throw new ArgumentNullException("coordinates");
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Deserializes the json into a Point
        /// </summary>
        /// <param name="json">The json to deserialize</param>
        /// <returns>A Point object</returns>
        public static new Point FromJson(string json)
        {
            return JsonConvert.DeserializeObject<Point>(json);
        }

        /// <summary>
        /// Gets the longitude or easting of the point
        /// </summary>
        /// <returns>The longitude</returns>
        public double GetLongitude()
        {
            return this.Coordinates.Longitude;
        }

        /// <summary>
        /// Gets the latitude or northing of the point
        /// </summary>
        /// <returns>The latitude</returns>
        public double GetLatitude()
        {
            return this.Coordinates.Latitude;
        }

        /// <summary>
        /// Gets the elevation of the point if it exists
        /// in the coordinates.
        /// </summary>
        /// <returns>The elevation or if it wasn't set, the returns NaN</returns>
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

            Point other = (Point)obj;

            bool bBoxEqual = true;

            if (this.BoundingBox != null && other.BoundingBox != null)
            {
                bBoxEqual = this.BoundingBox.SequenceEqual(other.BoundingBox);
            }
            else
            {
                bBoxEqual = (this.BoundingBox == null && other.BoundingBox == null);
            }

            return this.Type == other.Type &&
                this.Coordinates == other.Coordinates &&
                bBoxEqual;
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
