using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BAMCIS.GeoJSON
{
    /// <summary>
    /// A linear ring is a closed LineString with four or more positions.
    ///
    /// A linear ring is the boundary of a surface or the boundary of a
    /// hole in a surface.
    ///
    /// A linear ring MUST follow the right-hand rule with respect to the
    /// area it bounds, i.e., exterior rings are counterclockwise, and
    /// holes are clockwise.
    /// </summary>
    public class LinearRing : LineString
    {
        #region Constructors

        /// <summary>
        /// Creates a new LinearRing
        /// </summary>
        [JsonConstructor]
        public LinearRing(IEnumerable<Position> coordinates) : base(coordinates)
        {
            Position[] Coords = this.Coordinates.ToArray();

            if (Coords.Length < 4)
            {
                throw new ArgumentOutOfRangeException("coordinates", "A linear ring requires 4 or more positions.");
            }

            if (!Coords.First().Equals(Coords.Last()))
            {
                throw new ArgumentException("The first and last value must be equivalent.");
            }
        }

        #endregion
    }
}
