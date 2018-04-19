using Newtonsoft.Json;
using System;

namespace BAMCIS.GeoJSON
{
    /// <summary>
    /// For type "Point", the "coordinates" member is a single position.
    /// </summary>
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
        /// <param name="coordinates"></param>
        [JsonConstructor]
        public Point(Position coordinates) : base(GeometryType.Point)
        {
            this.Coordinates = coordinates ?? throw new ArgumentNullException("coordinates");
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the longitude or easting of the point
        /// </summary>
        /// <returns></returns>
        public double GetLongitude()
        {
            return this.Coordinates.Longitude;
        }

        /// <summary>
        /// Gets the latitude or northing of the point
        /// </summary>
        /// <returns></returns>
        public double GetLatitude()
        {
            return this.Coordinates.Latitude;
        }

        /// <summary>
        /// Gets the elevation of the point if it exists
        /// in the coordinates.
        /// </summary>
        /// <returns></returns>
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

        #endregion
    }
}
