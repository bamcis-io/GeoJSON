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
    public class Position : IPosition, IEquatable<Position>, IEqualityComparer<Position>
    {
        #region Public Properties

        /// <summary>
        /// The positions' longitude or easting
        /// </summary>
        public double Longitude { get; }

        /// <summary>
        /// The position's latitude or northing
        /// </summary>
        public double Latitude { get; }

        /// <summary>
        /// The positions elevation. This will be NaN 
        /// if an elevation is not provided for the position
        /// </summary>
        public double Elevation { get; }

        #endregion

        #region Constructors 

        /// <summary>
        /// Creates a position with a longitude and latitude
        /// </summary>
        /// <param name="longitude"></param>
        /// <param name="latitude"></param>
        public Position(double longitude, double latitude) : this(longitude, latitude, double.NaN)
        {
        }

        /// <summary>
        /// Creates a position with a longitude, latitude, and elevation
        /// </summary>
        /// <param name="longitude"></param>
        /// <param name="latitude"></param>
        /// <param name="elevation"></param>
        [JsonConstructor]
        public Position(double longitude, double latitude, double elevation)
        {
            if (double.IsInfinity(latitude) || double.IsNaN(latitude))
            {
                throw new ArgumentOutOfRangeException("Latitude", "The latitude cannot be NaN or infinity.");
            }

            if (double.IsInfinity(longitude) || double.IsNaN(longitude))
            {
                throw new ArgumentOutOfRangeException("Longitude", "The longitude cannot be NaN or infinity.");
            }

            if (double.IsInfinity(elevation))
            {
                throw new ArgumentOutOfRangeException("Elevation", "The elevation cannot be infinity.");
            }

            this.Latitude = latitude;
            this.Longitude = longitude;
            this.Elevation = elevation;
        }

        #endregion

        #region Public Methods

        public static Position FromJson(string json)
        {
            return JsonConvert.DeserializeObject<Position>(json);
        }

        /// <summary>
        /// Determines if an elevation has been provided for a position.
        /// </summary>
        /// <returns></returns>
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

            Position Other = (Position)obj;

            bool Temp = this.Latitude == Other.Latitude &&
               this.Longitude == Other.Longitude;

            if (!double.IsNaN(this.Elevation) || !double.IsNaN(Other.Elevation))
            {
                Temp = Temp && (this.Elevation == Other.Elevation);
            }

            return Temp;
        }

        public bool Equals(Position other)
        {
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            else
            {
                bool Temp = this.Latitude == other.Latitude &&
                this.Longitude == other.Longitude;

                if (!double.IsNaN(this.Elevation) || !double.IsNaN(other.Elevation))
                {
                    Temp = Temp && (this.Elevation == other.Elevation);
                }

                return Temp;
            }
        }

        public bool Equals(Position left, Position right)
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

        public static bool operator ==(Position left, Position right)
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

        public static bool operator !=(Position left, Position right)
        {
            return !(left == right);
        }

        public int GetHashCode(Position other)
        {
            return other.GetHashCode();
        }

        #endregion
    }
}
