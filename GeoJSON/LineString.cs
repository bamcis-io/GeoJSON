using System;
using System.Collections.Generic;
using System.Linq;

namespace BAMCIS.GeoJSON
{
    /// <summary>
    /// For type "LineString", the "coordinates" member is an array of two or
    /// more positions.
    /// </summary>
    public class LineString : Geometry
    {
        #region Public Properties

        /// <summary>
        /// The coordinates of a linestring are an array of positions
        /// </summary>
        public IEnumerable<Position> Cooridnates { get; }

        #endregion

        #region Constructors 

        /// <summary>
        /// Creates a new LineString
        /// </summary>
        /// <param name="coordinates"></param>
        public LineString(IEnumerable<Position> coordinates) : base(GeometryType.LineString)
        {
            this.Cooridnates = coordinates ?? throw new ArgumentNullException("coordinates");

            if (this.Cooridnates.Count() < 2)
            {
                throw new ArgumentOutOfRangeException("coordinates", "A LineString must have at least two positions.");
            }
        }

        #endregion
    }
}
