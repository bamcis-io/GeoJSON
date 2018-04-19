using System;
using System.Collections.Generic;

namespace BAMCIS.GeoJSON
{
    /// <summary>
    /// For type "MultiPoint", the "coordinates" member is an array of positions.
    /// </summary>
    public class MultiPoint : Geometry
    {
        #region Public Properties

        /// <summary>
        /// The coordinates of a multipoint are an array of positions
        /// </summary>
        public IEnumerable<Position> Cooridnates { get; }

        #endregion

        #region Constructors 

        /// <summary>
        /// Creates a new multipoint object
        /// </summary>
        /// <param name="coordinates"></param>
        public MultiPoint(IEnumerable<Position> coordinates) : base(GeometryType.MultiPoint)
        {
            this.Cooridnates = coordinates ?? throw new ArgumentNullException("coordinates");
        }

        #endregion
    }
}
