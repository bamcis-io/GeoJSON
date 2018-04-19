using BAMCIS.GeoJSON.Serde;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BAMCIS.GeoJSON
{
    /// <summary>
    /// For type "MultiLineString", the "coordinates" member is an array of
    /// LineString coordinate arrays.
    /// </summary>
    [JsonConverter(typeof(MultiLineStringConverter))]
    public class MultiLineString : Geometry
    {
        #region Public Properties

        /// <summary>
        /// For type "MultiLineString", the "coordinates" member is an array of
        /// LineString coordinate arrays.
        /// </summary>
        public IEnumerable<LineString> Cooridnates { get; }

        #endregion

        #region Constructors 

        /// <summary>
        /// Creates a new MultiLineString
        /// </summary>
        /// <param name="coordinates"></param>
        [JsonConstructor]
        public MultiLineString(IEnumerable<LineString> coordinates) : base(GeometryType.MultiLineString)
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
