using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BAMCIS.GeoJSON
{
    /// <summary>
    /// A GeoJSON object with type "GeometryCollection" is a Geometry object.
    /// A GeometryCollection has a member with the name "geometries".  The
    /// value of "geometries" is an array.
    /// </summary>
    public class GeometryCollection : Geometry
    {
        #region Public Properties

        /// <summary>
        /// The value of "geometries" is an array.Each element of this array is a
        /// GeoJSON Geometry object.  It is possible for this array to be empty.
        /// </summary>
        public IEnumerable<Geometry> Geometries { get; }

        #endregion

        #region Constructors 

        /// <summary>
        /// Creates a new GeometryCollection
        /// </summary>
        /// <param name="geometries"></param>
        [JsonConstructor]
        public GeometryCollection(IEnumerable<Geometry> geometries) : base(GeometryType.GeometryCollection)
        {
            this.Geometries = geometries ?? throw new ArgumentNullException("geometries");
        }

        #endregion
    }
}
