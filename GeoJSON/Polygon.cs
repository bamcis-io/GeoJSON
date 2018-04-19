using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BAMCIS.GeoJSON
{
    /// <summary>
    /// A polygon is formed with 1 or more linear rings, which are an enclosed LineString
    /// </summary>
    public class Polygon : Geometry
    {
        #region Public Properties

        /// <summary>
        /// The coordinates are an array of linear ring coordinate arrays.
        /// 
        /// For Polygons with more than one of these rings, the first MUST be
        /// the exterior ring, and any others MUST be interior rings.The
        /// exterior ring bounds the surface, and the interior rings(if
        /// present) bound holes within the surface.
        /// </summary>
        public IEnumerable<LinearRing> Cooridnates { get; }

        #endregion

        #region Constructors 

        /// <summary>
        /// Creates a new Polygon
        /// </summary>
        /// <param name="coordinates"></param>
        [JsonConstructor]
        public Polygon(IEnumerable<LinearRing> coordinates) : base(GeometryType.Polygon)
        {
            this.Cooridnates = coordinates ?? throw new ArgumentNullException("coordinates");

            if (!this.Cooridnates.Any())
            {
                throw new ArgumentOutOfRangeException("coordinates", "A polygon must have at least 1 linear ring.");
            }
        }

        #endregion
    }
}
