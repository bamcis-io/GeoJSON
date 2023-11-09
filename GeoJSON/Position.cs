using BAMCIS.GeoJSON.Serde;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BAMCIS.GeoJSON
{
    /// <summary>
    /// A GeoJSON position consisting of a longitude, latitude, and optional elevation
    /// </summary>
    [JsonConverter(typeof(PositionConverter))]
    public class Coordinate : IEquatable<Coordinate>, IEqualityComparer<Coordinate>
    {
        #region Public Properties

        /// <summary>
        /// The position's longitude or easting, valid values of
        /// -180 to 180. Set IgnorePositionValidation in the GeoJsonConfig
        /// class to ignore
        /// </summary>
        [JsonProperty(PropertyName = "longitude")]
        public double Longitude { get; }

        /// <summary>
        /// The position's latitude or northing
        /// </summary>
        [JsonProperty(PropertyName = "latitude")]
        public double Latitude { get; }

        /// <summary>
        /// The positions elevation. This will be NaN
        /// if an elevation is not provided for the position
        /// </summary>
        [JsonProperty(PropertyName = "elevation")]
        public double Elevation { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a position with a longitude and latitude
        /// </summary>
        /// <param name="longitude">The position's longitude</param>
        /// <param name="latitude">The position's latitude</param>
        public Coordinate(double longitude, double latitude) : this(longitude, latitude, double.NaN)
        {
        }

        /// <summary>
        /// Creates a position with a longitude, latitude, and elevation
        /// </summary>
        /// <param name="longitude">The position's longitude</param>
        /// <param name="latitude">The position's latitude</param>
        /// <param name="elevation">The position's elevation</param>
        [JsonConstructor]
        public Coordinate(double longitude, double latitude, double elevation)
        {
            if (double.IsInfinity(latitude) || double.IsNaN(latitude))
            {
                throw new ArgumentOutOfRangeException(nameof(latitude), "The latitude cannot be NaN or infinity.");
            }

            if (double.IsInfinity(longitude) || double.IsNaN(longitude))
            {
                throw new ArgumentOutOfRangeException(nameof(longitude), "The longitude cannot be NaN or infinity.");
            }

            if (!GeoJsonConfig.IgnoreLongitudeValidation)
            {
                if (longitude < -180 || longitude > 180)
                {
                    throw new ArgumentOutOfRangeException(nameof(longitude), "Longitude must be between -180 and 180 degrees, inclusive.");
                }
            }

            if (!GeoJsonConfig.IgnoreLatitudeValidation)
            {
                if (latitude < -90 || latitude > 90)
                {
                    throw new ArgumentOutOfRangeException(nameof(latitude), "Latitude must be between -90 and 90 degrees, inclusive.");
                }
            }

            if (double.IsInfinity(elevation))
            {
                throw new ArgumentOutOfRangeException(nameof(elevation), "The elevation cannot be infinity.");
            }

            this.Latitude = latitude;
            this.Longitude = longitude;
            this.Elevation = elevation;
        }

        #endregion

        #region Public Methods

        public double[] ToArray()
        {
            double[] array = new double[]{ this.Longitude, this.Latitude};

            return array;
        }

        /// <summary>
        /// Deserializes the provided json into a Position object
        /// </summary>
        /// <param name="json">The json to deserialize</param>
        /// <returns>A Position object</returns>
        public static Coordinate FromJson(string json)
        {
            return JsonConvert.DeserializeObject<Coordinate>(json);
        }

        /// <summary>
        /// Determines if an elevation has been provided for a position.
        /// </summary>
        /// <returns>Whether the position has a valid elevation value</returns>
        public bool HasElevation()
        {
            return !double.IsNaN(this.Elevation);
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

            Coordinate other = (Coordinate)obj;

            bool temp = this.Latitude == other.Latitude &&
               this.Longitude == other.Longitude;

            if (!double.IsNaN(this.Elevation) || !double.IsNaN(other.Elevation))
            {
                temp = temp && (this.Elevation == other.Elevation);
            }

            return temp;
        }

        public bool Equals(Coordinate other)
        {
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            else
            {
                bool temp = (this.Latitude == other.Latitude &&
                             this.Longitude == other.Longitude);

                if (!double.IsNaN(this.Elevation) || !double.IsNaN(other.Elevation))
                {
                    temp = temp && (this.Elevation == other.Elevation);
                }

                return temp;
            }
        }

        public bool Equals(Coordinate left, Coordinate right)
        {
            return left == right;
        }

        public override int GetHashCode()
        {
            return Hashing.Hash(this.Longitude, this.Latitude, this.Elevation);
        }

        public override string ToString()
        {
            return $"[{this.Longitude},{this.Latitude}{(!double.IsNaN(this.Elevation) ? $",{this.Elevation}" : "")}]";
        }

        public static bool operator ==(Coordinate left, Coordinate right)
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

        public static bool operator !=(Coordinate left, Coordinate right)
        {
            return !(left == right);
        }


        public static Coordinate operator -(Coordinate left, Coordinate right)
        {
            var newPosition = new Coordinate(left.Longitude - right.Longitude, left.Latitude - right.Latitude);

            return newPosition;
        }

        public static Coordinate operator -(Coordinate left, double translation)
        {
            var newPosition = new Coordinate(left.Longitude - translation, left.Latitude - translation);

            return newPosition;
        }

        public static Coordinate operator +(Coordinate left, Coordinate right)
        {
            var newPosition = new Coordinate(left.Longitude + right.Longitude, left.Latitude + right.Latitude);

            return newPosition;
        }

        public static Coordinate operator +(Coordinate left, double translation)
        {
            var newPosition = new Coordinate(left.Longitude + translation, left.Latitude + translation);

            return newPosition;
        }

        public static Coordinate operator *(Coordinate left, Coordinate right)
        {
            var newPosition = new Coordinate(left.Longitude * right.Longitude, left.Latitude * right.Latitude);

            return newPosition;
        }

        public static Coordinate operator *(Coordinate left, double translation)
        {
            var newPosition = new Coordinate(left.Longitude * translation, left.Latitude * translation);

            return newPosition;
        }

        public static Coordinate operator /(Coordinate left, Coordinate right)
        {
            var newPosition = new Coordinate(left.Longitude / right.Longitude, left.Latitude / right.Latitude);

            return newPosition;
        }

        public static Coordinate operator /(Coordinate left, double translation)
        {
            var newPosition = new Coordinate(left.Longitude / translation, left.Latitude / translation);

            return newPosition;
        }

        public int GetHashCode(Coordinate other)
        {
            return other.GetHashCode();
        }

        internal Coordinate Copy()
        {
            return new Coordinate(this.Longitude, this.Latitude);
        }

        internal Point ToPoint()
        {
            return new Point(this.Longitude, this.Latitude);
        }

        #endregion
    }
}
