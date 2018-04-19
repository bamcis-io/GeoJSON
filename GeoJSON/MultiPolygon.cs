using BAMCIS.GeoJSON.Serde;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BAMCIS.GeoJSON
{
    /// <summary>
    /// For type "MultiPolygon", the "coordinates" member is an array of
    /// Polygon coordinate arrays.
    /// </summary>
    [JsonConverter(typeof(MultiPolygonConverter))]
    public class MultiPolygon : Geometry
    {
        #region Public Properties

        /// <summary>
        /// The coordinates are an array of polygons.
        /// </summary>
        public IEnumerable<Polygon> Cooridnates { get; }

        #endregion

        #region Constructors 

        /// <summary>
        /// Creates a new MultiPolygon
        /// </summary>
        /// <param name="coordinates"></param>
        [JsonConstructor]
        public MultiPolygon(IEnumerable<Polygon> coordinates) : base(GeometryType.MultiPolygon)
        {
            this.Cooridnates = coordinates ?? throw new ArgumentNullException("coordinates");

            if (!this.Cooridnates.Any())
            {
                throw new ArgumentOutOfRangeException("coordinates", "A MultiPolygon must have at least 1 polygon.");
            }
        }

        #endregion
    }
}
