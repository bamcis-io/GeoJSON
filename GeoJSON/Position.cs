using BAMCIS.GeoJSON.Serde;
using Newtonsoft.Json;
using System;

namespace BAMCIS.GeoJSON
{
    /// <summary>
    /// A GeoJSON position consisting of a longitude, latitude, and optional elevation
    /// </summary>
    [JsonConverter(typeof(PositionConverter))]
    public class Position
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
            if (this == obj)
            {
                return true;
            }

            if (obj == null || this.GetType() != obj.GetType())
            {
                return false;
            }

            Position Other = (Position)obj;

            return this.Longitude == Other.Longitude &&
                this.Latitude == Other.Latitude &&
                this.Elevation == Other.Elevation;
        }

        public override int GetHashCode()
        {
            return Hashing.Hash(this.Longitude, this.Latitude, this.Elevation);
        }

        public override string ToString()
        {
            return $"[{this.Longitude},{this.Latitude}{(!double.IsNaN(this.Elevation) ? $",{this.Elevation}" : "")}]";
        }

        #endregion
    }
}
