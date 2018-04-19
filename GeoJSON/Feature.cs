using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BAMCIS.GeoJSON
{
    /// <summary>
    /// Feature objects in GeoJSON contain a Geometry object and additional members.
    /// </summary>
    public class Feature : GeoJson
    {
        #region Public Properties

        /// <summary>
        /// The geometry of this feature
        /// </summary>
        public Geometry Geometry { get; }

        /// <summary>
        /// Additional properties for the feature
        /// </summary>
        public IDictionary<string, string> Properties { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Builds a new Feature with the given geometry and optional properties
        /// </summary>
        /// <param name="geometry"></param>
        /// <param name="properties"></param>
        [JsonConstructor]
        public Feature(Geometry geometry, IDictionary<string, string> properties = null) : base(GeoJsonType.FEATURE)
        {
            this.Geometry = geometry ?? throw new ArgumentNullException("geometry");
            this.Properties = properties ?? new Dictionary<string, string>();
        }

        #endregion
    }
}
