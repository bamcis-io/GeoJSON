namespace BAMCIS.GeoJSON
{
    /// <summary>
    /// An interface for dealing with Positions
    /// </summary>
    public interface IPosition
    {
        /// <summary>
        /// Gets the elevation, returns NaN if there is no elevation
        /// </summary>
        double Elevation { get; }

        /// <summary>
        /// Gets the longitude of the position
        /// </summary>
        double Longitude { get; }

        /// <summary>
        /// Gets the latitude of the position
        /// </summary>
        double Latitude { get; }
    }
}
